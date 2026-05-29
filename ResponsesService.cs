using System;
using System.Collections.Generic;
using System.Speech.Synthesis;

namespace Professor_Bot_GUI
{
    public class ResponsesService
    {

        private Random rand = new Random();
        private SpeechSynthesizer speaker;

        private Dictionary<string, string> memory = new Dictionary<string, string>();

        private Dictionary<string, int> topicCount = new Dictionary<string, int>();

        public ResponsesService()
        {
            speaker = new SpeechSynthesizer();
            speaker.Volume = 100;
            speaker.Rate = 0;
        }



        private string[] happyResponses =
        {
            "Glad you're feeling good .. Stay safe online!",
            "Nice! Keep that positive energy going.",
            "Good to hear! Let’s keep your accounts secure too."
        };

        private string[] sadResponses =
        {
            "I'm here for you. Let’s make things a bit safer and easier ",
            "Sorry you're feeling that way. Want some cybersecurity tips to help you feel more in control?",
            "Take your time. I can help you stay protected online."
        };

        private string[] angryResponses =
        {
            "I understand you're frustrated. Let’s keep your data safe calmly.",
            "No stress — I’ll help you step by step.",
            "Let’s slow down and sort it out together."
        };

        private string[] greetings =
        {
            "Hello! I'm ready to help you stay secure online.",
            "Hi there! Ask me anything about cybersecurity.",
            "Welcome! Let's improve your online safety."
        };

        private string[] emptyResponses =
        {
            "You entered nothing. Please type a question.",
            "Oops! That was empty. Try asking something.",
            "Please type something so I can help you."
        };

        private string[] unknownResponses =
        {
            "Sorry, I didn't understand that.",
            "Can you rephrase your question?",
            "I'm not sure what you mean. Try asking differently."
        };

        private string[] passwordAdvice =
        {
            "Use strong passwords with at least 12 characters.",
            "Enable two-factor authentication whenever possible.",
            "Never reuse passwords across different accounts."
        };

        private string[] phishingAdvice =
        {
            "Never click suspicious email links.",
            "Check the sender's email carefully before responding.",
            "Phishing emails often create fake urgency."
        };

        private string[] vpnAdvice =
        {
            "A VPN helps protect your internet traffic.",
            "Use VPNs on public Wi-Fi networks.",
            "Choose trusted VPN providers with strong encryption."
        };

        private string[] privacyAdvice =
        {
            "Protect your personal data by reviewing privacy settings often.",
            "Avoid sharing sensitive information on public websites.",
            "Strong privacy settings help protect your online identity."
        };

        private string[] goodbyeResponses =
        {
            "Goodbye! Stay safe online.",
            "See you later. Keep your accounts secure.",
            "Bye! Remember to protect your personal information."
        };



        public string GetResponse(string input, string name)
        {
            string response = "";

            if (string.IsNullOrWhiteSpace(input))
            {

                response = GetRandom(emptyResponses);
            }
            else
            {
                input = input.ToLower();

                memory["name"] = name;


                string mood = DetectMood(input);

                if (mood == "happy")
                {

                    response = GetRandom(happyResponses);
                    Speak(response);
                    return response;

                }
                else if (mood == "sad")
                {

                    response = GetRandom(sadResponses);
                    Speak(response);
                    return response;

                }
                else if (mood == "angry")
                {

                    response = GetRandom(angryResponses);
                    Speak(response);
                    return response;

                }
                if (input.Contains("privacy"))
                {

                    memory["topic"] = "privacy";
                    TrackTopic("privacy");

                    response = HandleRepeat("privacy",
                        privacyAdvice,
                        "privacy");
                }
                else if (input.Contains("password"))
                {

                    memory["topic"] = "password security";
                    TrackTopic("password");

                    response = HandleRepeat("password",
                        passwordAdvice,
                        "password security");
                }

                else if (input.Contains("vpn") || input.Contains("authentication") || input.Contains("2fa"))
                {

                    memory["security"] = input;
                    TrackTopic("vpn");

                    response = HandleRepeat("vpn", vpnAdvice, "VPN & authentication");

                }
                else if (input.Contains("facebook") || input.Contains("instagram") ||input.Contains("tiktok"))
                {

                    memory["social"] = input;
                    response = "I will remember your social media interests.";
                }
                else if (input.Contains("recall") || input.Contains("remember"))
                {

                    response = RecallMemory();
                }

                else if (input.Contains("advice"))
                {
                    if (memory.ContainsKey("name") && memory.ContainsKey("topic"))
                    {

                        response = $"{memory["name"]}, since you're interested in {memory["topic"]}, always stay cautious online.";
                    }
                    else
                    {
                        response =
                            "I still need to learn more about your interests first.";
                    }
                }
                else if (input.Contains("hello") || input.Contains("hi") ||  input.Contains("hey"))
                {

                    response = GetRandom(greetings);

                }
                else if (input == "bye" || input == "exit" || input == "goodbye")
                {

                    response = $"Goodbye {name}! {GetRandom(goodbyeResponses)}";

                }
                else
                {
                    response = GetRandom(unknownResponses);
                }
            }

            Speak(response);
            return response;
        }


        private string DetectMood(string input)
        {
            if (input.Contains("sad") || input.Contains("depressed") ||input.Contains("upset"))
            {
                return "sad";
            }

            if (input.Contains("angry") || input.Contains("mad") || input.Contains("frustrated"))
            {
                return "angry";
            }

            if (input.Contains("happy") ||input.Contains("good") || input.Contains("great"))
            {
                return "happy";
            }

            return "neutral";
        }



        private string HandleRepeat(string key, string[] advice, string label)
        {
            if (topicCount[key] > 1)
            {
                return
                    $"You already asked about {label} before. " +
                    "Here is a reminder: " +
                    GetRandom(advice);
            }

            return GetRandom(advice);
        }

        private void TrackTopic(string topic)
        {
            if (topicCount.ContainsKey(topic))
            {
                topicCount[topic]++;
            }
            else
            {
                topicCount[topic] = 1;
            }
        }



        private string RecallMemory()
        {
            string response = "";

            if (memory.ContainsKey("name"))
            {
                response += "Name: " + memory["name"] + "\n";
            }
            if (memory.ContainsKey("topic"))
            {
                response += "Topic: " + memory["topic"] + "\n";
            }
            if (memory.ContainsKey("social"))
            {
                response += "Social Media: " + memory["social"] + "\n";
            }
            if (memory.ContainsKey("security"))
            {
                response += "Security: " + memory["security"] + "\n";
            }
            if (response == "")
            {
                return "I do not remember anything yet.";
            }
            return response;
        }



        private void Speak(string message)
        {
            speaker.SpeakAsyncCancelAll();
            speaker.SpeakAsync(message);
        }

        public void SpeakMessage(string message)
        {
            Speak(message);
        }

        private string GetRandom(string[] arr)
        {
            return arr[rand.Next(arr.Length)];
        }
    }
}