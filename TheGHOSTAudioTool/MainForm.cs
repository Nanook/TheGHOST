using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using Nanook.TheGhost.Plugins;
using System.Diagnostics;

namespace Nanook.TheGhost.AudioTool
{


    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
               
            imgMP3.Image = Properties.Resources.GreenTick;
            imgMP3.Visible = false;
            lblMp3.Enabled = false;
            imgOgg.Image = Properties.Resources.GreenTick;
            imgOgg.Visible = false;
            lblOgg.Enabled = false;
            imgWavDest.Image = Properties.Resources.GreenTick;
            imgWavDest.Visible = false;
            lblWavDest.Enabled = false;
            imgXbadPcm.Image = Properties.Resources.GreenTick;
            imgXbadPcm.Visible = false;
            lblXBADPCM.Enabled = false;

            try
            {
                TestMp3();
                imgMP3.Visible = true;
                lblMp3.Enabled = true;
            }
            catch (Exception ex)
            {
                lblComplete.Text = "Unable to play MP3: " + ex.Message;
                imgMP3.Image = Properties.Resources.redx;
                imgMP3.Visible = true;
                lblMp3.Enabled = true;
                imgOgg.Image = Properties.Resources.redx;
                imgOgg.Visible = true;
                lblOgg.Enabled = true;
                imgWavDest.Image = Properties.Resources.redx;
                imgWavDest.Visible = true;
                lblWavDest.Enabled = true;
                imgXbadPcm.Image = Properties.Resources.redx;
                imgXbadPcm.Visible = true;
                lblXBADPCM.Enabled = true;
                return;
            }
            finally
            {
                lblComplete.Visible = true;
            }

            try
            {
                TestOgg();
                imgOgg.Visible = true;
                lblOgg.Enabled = true;
            }
            catch (Exception ex)
            {
                lblComplete.Text = @"Unable to play Ogg: " + ex.Message;
                imgOgg.Image = Properties.Resources.redx;
                imgOgg.Visible = true;
                lblOgg.Enabled = true;
                imgWavDest.Image = Properties.Resources.redx;
                imgWavDest.Visible = true;
                lblWavDest.Enabled = true;
                imgXbadPcm.Image = Properties.Resources.redx;
                imgXbadPcm.Visible = true;
                lblXBADPCM.Enabled = true;
                return;
            }
            finally
            {
                lblComplete.Visible = true;
            }

            try
            {
                TestWavConvert();
                imgWavDest.Visible = true;
                lblWavDest.Enabled = true;
            }
            catch (Exception ex)
            {
                lblComplete.Text = "Unable to output Wav file: " + ex.Message;
                imgWavDest.Image = Properties.Resources.redx;
                imgWavDest.Visible = true;
                lblWavDest.Enabled = true;
                imgXbadPcm.Image = Properties.Resources.redx;
                imgXbadPcm.Visible = true;
                lblXBADPCM.Enabled = true;
                return;
            }
            finally
            {
                
                lblComplete.Visible = true;
            }

            try
            {
                TestXbadPcmConvert();
                TestXboxWavPlay();
                imgXbadPcm.Visible = true;
                lblXBADPCM.Enabled = true;
            }
            catch (Exception ex)
            {
                lblComplete.Text = "Unable to output XBADPCM Wav: " + ex.Message;
                imgXbadPcm.Image = Properties.Resources.redx;
                imgXbadPcm.Visible = true;
                lblXBADPCM.Enabled = true;
                return;
            }
            finally
            {
                lblComplete.Visible = true;
            }


            

            lblComplete.Text = "All tests have completed sucessfully. TheGHOST Should work for you!";

        }

        private void TestMp3()
        {
            PlayingForm frmp = new PlayingForm();
            frmp.Show();
            try
            {

                Application.DoEvents();
                AudioTests tester = new AudioTests();
                tester.PlayTest(@"sounds\test.mp3");
                frmp.Close();

                if (MessageBox.Show("Did you hear the audio?", "Did you hear?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //success
                }
                else
                {
                    throw new CantPlayMp3Exception("You couldn't hear the audio. Check you have an MP3 codec installed. Check the sound card is installed properly.");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                frmp.Close();
            }
        }


        private void TestXboxWavPlay()
        {
            PlayingForm frmp = new PlayingForm();
            frmp.Show();
            try
            {

                Application.DoEvents();

                AudioTests audioTester = new AudioTests();
                audioTester.PlayTest(@"sounds\testXbadPcmOutput.wav");

                frmp.Close();

                if (MessageBox.Show("Did you hear the audio?", "Did you hear?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //success
                }
                else
                {
                    throw new CantPlayMp3Exception("You couldn't hear the converted XBADPCM audio. Make sure you have the codec installed correctly and the Priority set to 1");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                frmp.Close();
            }
        }

        private void TestOgg()
        {
            PlayingForm frmp = new PlayingForm();
            frmp.Show();
            try
            {
                
                Application.DoEvents();

                AudioTests audioTester = new AudioTests();
                audioTester.PlayTest(Application.StartupPath + @"\sounds\test.ogg");

                frmp.Close();

                if (MessageBox.Show("Did you hear the audio?", "Did you hear?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    // success
                }
                else
                {
                    throw new OggPlaybackException("You couldn't hear the audio. Check the Ogg codec is installed");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                frmp.Close();
            }
        }

        private void TestWavConvert()
        {
            PlayingForm frmp = new PlayingForm();
            frmp.Show();
            try
            {
                FileHelper.Delete(@"sounds\testOutput.wav");
                                
                Application.DoEvents();

                IPluginAudioImport p = new WindowsAudio();
                p.Convert(@"sounds\test.mp3", @"sounds\testOutput.wav");
                

                frmp.Close();

                if (File.Exists(@"sounds\testOutput.wav"))
                {
                    FileInfo fi = new FileInfo(@"sounds\testOutput.wav");
                    if (fi.Length > 500000)
                    {
                        //success

                    }
                    else
                    {
                        throw new CantConvertMP3Exception("MP3 to Wav Conversion failed. Is the program path writeable?");
                    }
                }
                else
                {
                    throw new CantConvertMP3Exception("Output Wav not created. Is the program path writeable?");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                frmp.Close();
            }
        }

        private void TestXbadPcmConvert()
        {
            PlayingForm frmp = new PlayingForm();
            frmp.Show();
            try
            {
                FileHelper.Delete(@"sounds\testXbadPcmOutput.wav");
           
                Application.DoEvents();

                IPluginAudioExport p = new Xbadpcm();
                p.Convert(@"sounds\testOutput.wav", @"sounds\testXbadPcmOutput.wav", false, 0);
                
                frmp.Close();

                if (File.Exists(@"sounds\testXbadPcmOutput.wav"))
                {
                    FileInfo fi = new FileInfo(@"sounds\testXbadPcmOutput.wav");
                    if (fi.Length > 100000)
                    {
                        // success

                    }
                    else
                    {
                        throw new XbadPcmConvertException("Xbox PCM conversion failed. Is XBADPCM codec installed?");
                    }
                }
                else
                {
                    throw new XbadPcmConvertException("Xbox PCM output file not created. Is program path writeable?");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                frmp.Close();                        
                //FileHelper.Delete("testXbadPcmOutput.wav");
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            FileHelper.Delete(@"sounds\testOutput.wav");
            FileHelper.Delete(@"sounds\testXbadPcmOutput.wav");            

        }

        private void btnInstallWavDest_Click(object sender, EventArgs e)
        {
            string path = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location).DirectoryName.TrimEnd('\\');
            string regsvrPath = string.Empty;

            if (OsInfo.GetPlatform() == Platform.X86)
                regsvrPath = Environment.SystemDirectory;
            else if (OsInfo.GetPlatform() == Platform.X64)
                regsvrPath = string.Format(@"{0}\syswow64", Environment.ExpandEnvironmentVariables("%windir%"));

            try
            {
                File.Copy(string.Format(@"{0}\wavdest\wavdest.ax", path), string.Format(@"{0}\wavdest.ax", regsvrPath), true);
            }
            catch
            {
            }
            
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = string.Format(@"{0}\regsvr32.exe", regsvrPath);
            psi.Arguments = string.Format(@" ""{0}\wavdest.ax""", regsvrPath);
            Process.Start(psi);

        }

        private void btnInstallXbadpcm_Click(object sender, EventArgs e)
        {
            string path = new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location).DirectoryName.TrimEnd('\\');
            string rundllPath = string.Empty;

            if (OsInfo.GetPlatform() == Platform.X86)
                rundllPath = Environment.SystemDirectory;
            else if (OsInfo.GetPlatform() == Platform.X64)
                rundllPath = string.Format(@"{0}\syswow64", Environment.ExpandEnvironmentVariables("%windir%"));

            try
            {
                File.Copy(string.Format(@"{0}\xbadpcm\xbadpcminst.inf", path), string.Format(@"{0}\xbadpcminst.inf", rundllPath), true);
            }
            catch
            {
            }
            try
            {
                File.Copy(string.Format(@"{0}\xbadpcm\XBADPCM.ACM", path), string.Format(@"{0}\XBADPCM.ACM", rundllPath), true);
            }
            catch
            {
            }

            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = string.Format(@"{0}\rundll32.exe", rundllPath);
            psi.Arguments = string.Format(@"setupapi.dll,InstallHinfSection DefaultInstall 0 {0}\xbadpcminst.inf", rundllPath);
            Process.Start(psi);
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.Text = string.Format("{0} - v{1} / v{2}", TheGhostCore.AppName, TheGhostCore.AppVersion, TheGhostCore.CoreVersion);
        }

    }
}
