/* MIT License

Copyright (c) 2021 Glenn Alon

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE. */

using System.Text;

namespace translator
{
    public partial class Form1 : Form
    {
        public string word { get; set; }
        public string a { get; set; }
        public string b { get; set; }
        public string i { get; set; }
        public string j { get; set; }

        public Form1()
        {
            InitializeComponent();
        }

        public static string Translate(string WORD, string FROM, string TO)
        {
            var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={FROM}&ie=UTF8&tl={TO}&dt=t&q={Uri.EscapeDataString(WORD)}";
            HttpClient httpClient = new();
            try
            {
                string result = httpClient.GetStringAsync(url).Result;
                result = result[4..result.IndexOf("\"", 4, StringComparison.Ordinal)];
                string utfResult;
                byte[] bytes = Encoding.Default.GetBytes(result);
                utfResult = Encoding.UTF8.GetString(bytes);
                return utfResult;
            }
            catch
            {
                return "Error!";
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            string result = Translate(word, i, j);
            string decodedResult;
            byte[] bytes = Encoding.Default.GetBytes(result);
            decodedResult = Encoding.UTF8.GetString(bytes);
            textBox2.Text = decodedResult;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf("English");
            comboBox2.SelectedIndex = comboBox2.Items.IndexOf("Filipino");
            textBox1.Text = "This is a sample text.";
            SetLanguageFrom();
            SetLanguageTo();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            word = textBox1.Text;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetLanguageFrom();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetLanguageTo();
        }
        private void SetLanguageFrom()
        {
            a = comboBox1.GetItemText(comboBox1.SelectedItem);

            switch (a)
            {
                case "English":
                    i = "en";
                    break;
                case "Filipino":
                    i = "fil";
                    break;
                case "Japanese":
                    i = "ja";
                    break;
                case "Korean":
                    i = "ko";
                    break;
            }
        }
        private void SetLanguageTo()
        {
            b = comboBox2.GetItemText(comboBox2.SelectedItem);

            switch (b)
            {
                case "English":
                    j = "en";
                    break;
                case "Filipino":
                    j = "fil";
                    break;
                case "Japanese":
                    j = "ja";
                    break;
                case "Korean":
                    j = "ko";
                    break;
            }
        }
    }
}
