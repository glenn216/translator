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

using System.Net.NetworkInformation;

namespace translator
{
    public partial class Form1 : Form
    {
        public string? Word { get; set; }
        public string? A { get; set; }
        public string? B { get; set; }
        public string? I { get; set; }
        public string? J { get; set; }
        private const int StartIndex = 4;

        public Form1() => InitializeComponent();

        public static string Translate(string WORD, string FROM, string TO)
        {
            var url = $"https://translate.googleapis.com/translate_a/single?client=gtx&sl={FROM}&ie=UTF8&tl={TO}&dt=t&q={Uri.EscapeDataString(WORD)}";
            HttpClient httpClient = new();
            try
            {
                string result = httpClient.GetStringAsync(url).Result; // Fetch the result from the Google Translate API.
                result = result[StartIndex..result.IndexOf("\"", StartIndex, StringComparison.Ordinal)]; // Only the necessary string is returned.
                return result;
            }
            catch
            {
                return "Error!";
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false; // Disable button to prevent multiple queries at once.
            Task.Run(() => Ping("www.google.com", 2000)); // Ping the host for 2 seconds. Checks for an active internet connection.
            string result = Translate(Word, I, J); // Translate(WORD, FROM, TO)
            textBox2.Text = result; // Display the translated output into textBox2.
            button1.Enabled = true; // Enable the button.
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = comboBox1.Items.IndexOf("English"); // Set the default language translated FROM. 
            comboBox2.SelectedIndex = comboBox2.Items.IndexOf("Filipino"); // Set the default language translated TO.
            textBox1.Text = "This is a sample text.";
            SetLanguageFrom(); // The string values are updated as a result of this.
            SetLanguageTo(); // The same is applicable here.
        }

        private void TextBox1_TextChanged(object sender, EventArgs e) => Word = textBox1.Text; // If the text has changed, update the string.

        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e) => SetLanguageFrom(); // If the value of comboBox1 changes, the strings will be updated.

        private void ComboBox2_SelectedIndexChanged(object sender, EventArgs e) => SetLanguageTo(); // The same is applicable for comboBox2.
       
        private void SetLanguageFrom()
        {
            A = comboBox1.GetItemText(comboBox1.SelectedItem);

            I = A switch
            {
                "English" => "en", // If English is selected in comboBox1, I = "en".
                "Filipino" => "fil", 
                "Japanese" => "ja",
                "Korean" => "ko",
                "Auto" => "auto",
                _ => "auto", // Set to auto-detect if comboBox1 is empty.
            };
        }

        private void SetLanguageTo()
        {
            B = comboBox2.GetItemText(comboBox2.SelectedItem);

            J = B switch
            {
                "English" => "en", // If English is selected in comboBox2, J = "en".
                "Filipino" => "fil",
                "Japanese" => "ja",
                "Korean" => "ko",
                _ => "en", // Set to English if comboBox2 is empty.
            };
        }

        public static async Task Ping(string host, int timeout)
        {
            try
            {
                _ = (await new Ping().SendPingAsync(host, timeout)).Status == IPStatus.Success;

            }

            catch (Exception)
            {
                 _ = MessageBox.Show("Ensure that you are connected to the internet.", "Internet Connection Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
             }
        }
    }
}

