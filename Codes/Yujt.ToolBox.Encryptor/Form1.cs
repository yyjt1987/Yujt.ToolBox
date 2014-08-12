using System;
using System.Windows.Forms;
using Yujt.Common.Encrypts;

namespace yujt.Encryptor
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void EncryptButton_Click(object sender, EventArgs e)
        {
            var encryptingStr = EncryptingText.Text;
            var encyptedStr = DesEncyptor.Encrypt(encryptingStr);
            DecryptingText.Text = encyptedStr;
        }

        private void DecryptButton_Click(object sender, EventArgs e)
        {
            var decryptingStr = DecryptingText.Text;
            if (string.IsNullOrEmpty(decryptingStr))
            {
                MessageBox.Show("不能为空.");
                return;
            }

            try
            {
                var decyptedStr = DesEncyptor.Decrypt(decryptingStr);
                EncryptingText.Text = decyptedStr;
            }
            catch
            {
                MessageBox.Show("无法解密.");
            }
        }

    }
}
