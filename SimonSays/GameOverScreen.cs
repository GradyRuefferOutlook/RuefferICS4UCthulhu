using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimonSays
{
    public partial class GameOverScreen : UserControl
    {
        //Initialize Sounds Locally
        public static System.Windows.Media.MediaPlayer redSound = new System.Windows.Media.MediaPlayer();
        public static System.Windows.Media.MediaPlayer orangeSound = new System.Windows.Media.MediaPlayer();
        public static System.Windows.Media.MediaPlayer yellowSound = new System.Windows.Media.MediaPlayer();
        public static System.Windows.Media.MediaPlayer greenSound = new System.Windows.Media.MediaPlayer();
        public static System.Windows.Media.MediaPlayer blueSound = new System.Windows.Media.MediaPlayer();
        public static System.Windows.Media.MediaPlayer pinkSound = new System.Windows.Media.MediaPlayer();
        public static System.Windows.Media.MediaPlayer purpleSound = new System.Windows.Media.MediaPlayer();
        public static System.Windows.Media.MediaPlayer octarineSound = new System.Windows.Media.MediaPlayer();
        public GameOverScreen()
        {
            InitializeComponent();
        }

        private void GameOverScreen_Load(object sender, EventArgs e)
        {
            //Enter the length of the pattern
            lengthLabel.Text = $"{Form1.cthulhuPattern.Count - 1}";

            //Start the operator
            EndOp.Enabled = true;

            //Play all notes for loss sound
            redSound.Open(new Uri(Application.StartupPath + "\\Resources\\church-choir-note-c_91bpm_C_major.wav"));
            redSound.Play();

            orangeSound.Open(new Uri(Application.StartupPath + "\\Resources\\church-choir-note-d_96bpm_D_major.wav"));
            orangeSound.Play();

            yellowSound.Open(new Uri(Application.StartupPath + "\\Resources\\church-choir-note-e_66bpm_E_major.wav"));
            yellowSound.Play();

            greenSound.Open(new Uri(Application.StartupPath + "\\Resources\\church-choir-note-f_90bpm_F_major.wav"));
            greenSound.Play();

            blueSound.Open(new Uri(Application.StartupPath + "Resources\\church-choir-note-g_130bpm_G_major.wav"));
            blueSound.Play();

            pinkSound.Open(new Uri(Application.StartupPath + "\\Resources\\church-choir-note-g-2_69bpm_G#.wav"));
            pinkSound.Play();

            purpleSound.Open(new Uri(Application.StartupPath + "\\Resources\\church-choir-note-a_89bpm_A_major.wav"));
            purpleSound.Play();

            octarineSound.Open(new Uri(Application.StartupPath + "\\Resources\\church-choir-note-b_112bpm_B_major.wav"));
            octarineSound.Play();
        }

        private void EndOp_Tick(object sender, EventArgs e)
        {
            //Check if any button has been pressed and send back to the menu
            if (!Form1.isOver)
            {
                Form1.ScreenChanger(new LoadingScreen(), this);
                Form1.continueSound.Open(new Uri(Application.StartupPath + "\\Resources\\ghost-whispers-6030 (mp3cut.net).wav"));
                Form1.continueSound.Play();
                LoadingScreen.sentFromMenu = false;
                EndOp.Enabled = false;
                return;
            }
            Refresh();
        }
    }
}
