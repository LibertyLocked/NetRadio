using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;
using System.IO;
using System.Net;
using System.Diagnostics;
using System.Threading;

namespace RadioPlayer
{
    class ShoutcastPlayer
    {
        private BufferedWaveProvider bufferedWaveProvider;
        private IWavePlayer waveOut;
        private volatile StreamingPlaybackState playbackState;
        private volatile bool fullyDownloaded;
        private HttpWebRequest webRequest;
        private VolumeWaveProvider16 volumeProvider;
        private float volume;

        public string StreamTitle
        {
            get;
            private set;
        }

        public float Volume
        {
            get { return volume; }
            set
            {
                if (value > 1f) volume = 1f;
                else if (value < 0) volume = 0;
                else volume = value;
            }
        }

        public StreamingPlaybackState State
        {
            get { return playbackState; }
        }

        public ShoutcastPlayer()
        {
            volume = 1f;
        }

        public void StartPlayback(string url)
        {
            if (playbackState == StreamingPlaybackState.Stopped)
            {
                playbackState = StreamingPlaybackState.Buffering;
                bufferedWaveProvider = null;
                ThreadPool.QueueUserWorkItem(StreamMp3, url);
                //timer1.Enabled = true;
            }
            else if (playbackState == StreamingPlaybackState.Paused)
            {
                playbackState = StreamingPlaybackState.Buffering;
            }
        }

        public void PausePlayback()
        {
            if (playbackState == StreamingPlaybackState.Playing || playbackState == StreamingPlaybackState.Buffering)
            {
                waveOut.Pause();
                Debug.WriteLine(String.Format("User requested Pause, waveOut.PlaybackState={0}", waveOut.PlaybackState));
                playbackState = StreamingPlaybackState.Paused;
            }
        }

        public void StopPlayback()
        {
            if (playbackState != StreamingPlaybackState.Stopped)
            {
                if (!fullyDownloaded)
                {
                    webRequest.Abort();
                }
                
                playbackState = StreamingPlaybackState.Stopped;
                if (waveOut != null)
                { 
                    waveOut.Stop();
                    waveOut.Dispose();
                    waveOut = null;
                }
                //timer1.Enabled = false;
                // n.b. streaming thread may not yet have exited
                Thread.Sleep(500);
            }
        }

        public void Update()
        {
            if (playbackState != StreamingPlaybackState.Stopped)
            {
                if (waveOut == null && bufferedWaveProvider != null)
                {
                    Debug.WriteLine("Creating WaveOut Device");
                    waveOut = CreateWaveOut(); 
                    waveOut.PlaybackStopped += OnPlaybackStopped;
                    volumeProvider = new VolumeWaveProvider16(bufferedWaveProvider);
                    volumeProvider.Volume = Volume;
                    waveOut.Init(volumeProvider);
                }
                else if (bufferedWaveProvider != null)
                {
                    volumeProvider.Volume = Volume;

                    var bufferedSeconds = bufferedWaveProvider.BufferedDuration.TotalSeconds;
                    // make it stutter less if we buffer up a decent amount before playing
                    if (bufferedSeconds < 0.5 && playbackState == StreamingPlaybackState.Playing && !fullyDownloaded)
                    {
                        Pause();
                    }
                    else if (bufferedSeconds > 4 && playbackState == StreamingPlaybackState.Buffering)
                    {
                        Play();
                    }
                    else if (fullyDownloaded && bufferedSeconds == 0)
                    {
                        Debug.WriteLine("Reached end of stream");
                        StopPlayback();
                    }
                }

            }
        }

        private void StreamMp3(object state)
        {
            fullyDownloaded = false;
            var url = (string)state;
            webRequest = (HttpWebRequest)WebRequest.Create(url);

            // To retrieve ShoutCAST metadata
            int metaInt = 0; // blocksize of mp3 data
            webRequest.Headers.Clear();
            webRequest.Headers.Add("GET", "/ HTTP/1.0");
            // needed to receive metadata informations
            webRequest.Headers.Add("Icy-MetaData", "1");
            webRequest.UserAgent = "WinampMPEG/5.09";

            HttpWebResponse resp;
            try
            {
                resp = (HttpWebResponse)webRequest.GetResponse();
            }
            catch (WebException e)
            {
                if (e.Status != WebExceptionStatus.RequestCanceled)
                {
                    throw;
                    //ShowError(e.Message);
                }
                return;
            }
            var buffer = new byte[16384 * 4]; // needs to be big enough to hold a decompressed frame

            try
            {
                // read blocksize to find metadata block
                metaInt = Convert.ToInt32(resp.GetResponseHeader("icy-metaint"));
            }
            catch
            {
            }


            IMp3FrameDecompressor decompressor = null;
            try
            {
                using (var responseStream = resp.GetResponseStream())
                {
                    var readFullyStream = new ShoutcastStream(responseStream);
                    readFullyStream.MetaInt = metaInt;

                    do
                    {
                        if (IsBufferNearlyFull)
                        {
                            Debug.WriteLine("Buffer getting full, taking a break");
                            Thread.Sleep(500);
                        }
                        else
                        {
                            Mp3Frame frame;
                            try
                            {
                                frame = Mp3Frame.LoadFromStream(readFullyStream);

                                if (metaInt > 0)
                                    StreamTitle = (readFullyStream.StreamTitle);
                                else
                                    StreamTitle = "Title cannot be retrieved";
                            }
                            catch (EndOfStreamException)
                            {
                                fullyDownloaded = true;
                                // reached the end of the MP3 file / stream
                                break;
                            }
                            catch (WebException)
                            {
                                // probably we have aborted download from the GUI thread
                                break;
                            }
                            if (decompressor == null)
                            {
                                // don't think these details matter too much - just help ACM select the right codec
                                // however, the buffered provider doesn't know what sample rate it is working at
                                // until we have a frame
                                decompressor = CreateFrameDecompressor(frame);
                                bufferedWaveProvider = new BufferedWaveProvider(decompressor.OutputFormat);
                                bufferedWaveProvider.BufferDuration = TimeSpan.FromSeconds(20); // allow us to get well ahead of ourselves
                                //this.bufferedWaveProvider.BufferedDuration = 250;
                            }
                            try
                            {
                                int decompressed = decompressor.DecompressFrame(frame, buffer, 0);
                                //Debug.WriteLine(String.Format("Decompressed a frame {0}", decompressed));
                                bufferedWaveProvider.AddSamples(buffer, 0, decompressed);
                            }
                            catch
                            {
                                if (this.State == StreamingPlaybackState.Stopped) return;
                                else throw;
                            }
                        }

                    } while (playbackState != StreamingPlaybackState.Stopped);
                    Debug.WriteLine("Exiting");
                    // was doing this in a finally block, but for some reason
                    // we are hanging on response stream .Dispose so never get there
                    decompressor.Dispose();
                }
            }
            finally
            {
                if (decompressor != null)
                {
                    decompressor.Dispose();
                }
            }
        }

        private static IMp3FrameDecompressor CreateFrameDecompressor(Mp3Frame frame)
        {
            WaveFormat waveFormat = new Mp3WaveFormat(frame.SampleRate, frame.ChannelMode == ChannelMode.Mono ? 1 : 2,
                frame.FrameLength, frame.BitRate);
            return new AcmMp3FrameDecompressor(waveFormat);
        }

        private bool IsBufferNearlyFull
        {
            get
            {
                return bufferedWaveProvider != null &&
                       bufferedWaveProvider.BufferLength - bufferedWaveProvider.BufferedBytes
                       < bufferedWaveProvider.WaveFormat.AverageBytesPerSecond / 4;
            }
        }

        private void Play()
        {
            waveOut.Play();
            Debug.WriteLine(String.Format("Started playing, waveOut.PlaybackState={0}", waveOut.PlaybackState));
            playbackState = StreamingPlaybackState.Playing;
        }

        private void Pause()
        {
            playbackState = StreamingPlaybackState.Buffering;
            waveOut.Pause();
            Debug.WriteLine(String.Format("Paused to buffer, waveOut.PlaybackState={0}", waveOut.PlaybackState));
        }

        private IWavePlayer CreateWaveOut()
        {
            return new WaveOut();
        }

        private void MP3StreamingPanel_Disposing(object sender, EventArgs e)
        {
            StopPlayback();
        }

        private void OnPlaybackStopped(object sender, StoppedEventArgs e)
        {
            Debug.WriteLine("Playback Stopped");
            if (e.Exception != null)
            {
                throw e.Exception;
                //MessageBox.Show(String.Format("Playback Error {0}", e.Exception.Message));
            }
        }
    }
}
