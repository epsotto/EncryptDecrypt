using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EncryptDecrypt
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            String hashKey = textBox1.Text;
            String originalText = textBox2.Text;

            textBox3.Text = EncryptText(hashKey, originalText);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            String hashKey = textBox1.Text;
            String originalText = textBox2.Text;

            textBox4.Text = DecryptText(hashKey, originalText);
        }

        private string DecryptText(string hashKey, string originalText)
        {
            String decryptedText = String.Empty;

            byte[] data = Convert.FromBase64String(originalText);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hashKey));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateDecryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    decryptedText = UTF8Encoding.UTF8.GetString(results);
                }
            }

            return decryptedText;
        }

        private string EncryptText(string hashKey, string originalText)
        {
            String encryptedText = String.Empty;

            byte[] data = UTF8Encoding.UTF8.GetBytes(originalText);
            using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
            {
                byte[] keys = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(hashKey));
                using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                {
                    ICryptoTransform transform = tripDes.CreateEncryptor();
                    byte[] results = transform.TransformFinalBlock(data, 0, data.Length);
                    encryptedText = Convert.ToBase64String(results, 0, results.Length);
                }
            }

            return encryptedText;
        }
    }
}
