using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Locker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            optionComboBox.SelectedIndex = 0;
        }

        private void OptionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            inputTextBox.Clear();
            outputTextBox.Clear();
            openFileDialog1.FileName = "";
            saveFileDialog1.FileName = "";
            ComboBox comboBox = sender as ComboBox;
            if(comboBox.SelectedIndex == 0)
            {
                descripionLabel.Text = "Plik do odszyfrowania";
                cryptButton.Text = "Odszyfruj";
                saveFileDialog1.Filter = "Wszystkie pliki (*.*)|*.*";
                saveFileDialog1.DefaultExt = "";
                openFileDialog1.Filter = "Pliki Locker (*.lck)|*.lck";
                openFileDialog1.DefaultExt = "lck";
            }
            else
            {
                descripionLabel.Text = "Plik do zaszyfrowania";
                cryptButton.Text = "Zaszyfruj";
                saveFileDialog1.Filter = "Pliki Locker(*.lck) | *.lck";
                saveFileDialog1.DefaultExt = "lck";
                openFileDialog1.Filter = "Wszystkie pliki (*.*)|*.*";
                openFileDialog1.DefaultExt = "";
            }
        }

        private void InputTextBox_DoubleClick(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                inputTextBox.Text = openFileDialog1.FileName;
            }
        }

        private void OutputTextBox_DoubleClick(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                outputTextBox.Text = saveFileDialog1.FileName;
            }
        }


        private void CryptButton_Click(object sender, EventArgs e)
        {
            if (inputTextBox.TextLength > 0 && outputTextBox.TextLength > 0 && passwordTextBox.TextLength > 0)
            {
                viewPanel.Enabled = false;
                if (optionComboBox.SelectedIndex == 0)
                {
                    Cryptography.DecryptFile(passwordTextBox.Text, inputTextBox.Text, outputTextBox.Text);
                }
                else
                {
                    Cryptography.EncryptFile(passwordTextBox.Text, inputTextBox.Text, outputTextBox.Text);
                }
                viewPanel.Enabled = true;
            }
            else
            {
                MessageBox.Show("Trzeba wpisać hasło i wybrać ścierzki plików!", "Uwaga!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void PasswordTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                CryptButton_Click(null, null);
            }
        }
    }
}
