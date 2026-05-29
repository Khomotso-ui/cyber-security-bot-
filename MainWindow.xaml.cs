using Professor_Bot_GUI;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

namespace chat_part2
{
    public partial class MainWindow : Window
    {
        ResponsesService response = new ResponsesService();

        
        private string userName = "";

        public MainWindow()
        {
            InitializeComponent();

           
            Paragraph startParagraph = new Paragraph();

            Run botName = new Run("Bot: ");
            botName.Foreground = Brushes.LimeGreen;
            botName.FontWeight = FontWeights.Bold;

            Run botText = new Run("Please enter your name first.");
            botText.Foreground = Brushes.White;

            startParagraph.Inlines.Add(botName);
            startParagraph.Inlines.Add(botText);

            txtChat.Document.Blocks.Add(startParagraph);

          
            response.SpeakMessage("Please enter your name first.");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string userMessage = txtMessage.Text;

            if (string.IsNullOrWhiteSpace(userMessage))
            {
                return;
            }

          
            if (string.IsNullOrWhiteSpace(userName))
            {
                userName = userMessage;

        
                Paragraph userParagraph = new Paragraph();

                Run userLabel = new Run(userName + ": ");
                userLabel.Foreground = Brushes.Red;
                userLabel.FontWeight = FontWeights.Bold;

                Run userText = new Run(userName);
                userText.Foreground = Brushes.White;

                userParagraph.Inlines.Add(userLabel);
                userParagraph.Inlines.Add(userText);

                txtChat.Document.Blocks.Add(userParagraph);

              
                Paragraph welcomeParagraph = new Paragraph();

                Run botLabel = new Run("Bot: ");
                botLabel.Foreground = Brushes.LimeGreen;
                botLabel.FontWeight = FontWeights.Bold;

                string welcomeMessage =
                    "Hello " + userName +
                    ". Welcome to Chant-Bot, a cybersecurity awareness bot. ";

                Run welcomeText = new Run(welcomeMessage);
                welcomeText.Foreground = Brushes.White;

                welcomeParagraph.Inlines.Add(botLabel);
                welcomeParagraph.Inlines.Add(welcomeText);

                txtChat.Document.Blocks.Add(welcomeParagraph);

        
                response.SpeakMessage(welcomeMessage);

                txtMessage.Clear();
                return;
            }

        
            Paragraph normalUserParagraph = new Paragraph();

            Run normalUserLabel = new Run(userName + ": ");
            normalUserLabel.Foreground = Brushes.Red;
            normalUserLabel.FontWeight = FontWeights.Bold;

            Run normalUserText = new Run(userMessage);
            normalUserText.Foreground = Brushes.White;

            normalUserParagraph.Inlines.Add(normalUserLabel);
            normalUserParagraph.Inlines.Add(normalUserText);

            txtChat.Document.Blocks.Add(normalUserParagraph);

            
            string botReply = response.GetResponse(userMessage, userName);

            Paragraph botParagraph = new Paragraph();

            Run botReplyLabel = new Run("Bot: ");
            botReplyLabel.Foreground = Brushes.LimeGreen;
            botReplyLabel.FontWeight = FontWeights.Bold;

            Run botReplyText = new Run(botReply);
            botReplyText.Foreground = Brushes.White;

            botParagraph.Inlines.Add(botReplyLabel);
            botParagraph.Inlines.Add(botReplyText);

            txtChat.Document.Blocks.Add(botParagraph);

            txtMessage.Clear();
        }
    }
}