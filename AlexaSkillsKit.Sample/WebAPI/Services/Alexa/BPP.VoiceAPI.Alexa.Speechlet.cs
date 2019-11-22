using System;
using System.Collections.Generic;
using AlexaSkillsKit.Speechlet;
using AlexaSkillsKit.Slu;
using AlexaSkillsKit.UI;

namespace BPP.VoiceAPI.Services
{
    public class AlexaSpeechletService : Speechlet
    {

        // BPP SLOTS
        private const string QUERY_FIRSTLAST_SLOT = "Timetable_QueryFirstLast";
        private const string QUERY_TIMES_SLOT = "Timetable_QueryTimes";
        private const string QUERY_DATETIME = "Date";



        public override void OnSessionStarted(SessionStartedRequest request, Session session)
        {

        }

        public override SpeechletResponse OnLaunch(LaunchRequest request, Session session)
        {
            return GetWelcomeResponse();

        }
        public override void OnSessionEnded(SessionEndedRequest request, Session session)
        {
            
        }

        private string RandomDone()
        {

            int _int = 0;

            Random _rnd = new Random();
            _int = _rnd.Next(1, 10);

            switch (_int)
            {
                case 1:
                    return "Ok.";
                case 2:
                    return "Done.";
                case 3:
                    return "K.";
                case 4:
                    return "Yep.";
                case 5:
                    return "No problem.";
                case 6:
                    return "Okey dokey.";
                case 7:
                    return "Check.";
                case 8:
                    return "Sure.";
                case 9:
                    return "That's done.";
                case 10:
                    return "Ok.";
                default:
                    return "Ok done.";

            }
        }


        public override SpeechletResponse OnIntent(IntentRequest request, Session session)
        {

            
            // Get intent from the request.
            Intent intent = request.Intent;
            string intentName = (intent != null) ? intent.Name : null;


            // Find which intent has been requested and process.
            if ("Timetable_DateQuery".Equals(intentName))
            {
                return Timetable_DateQuery(intent, session);
            }
            else if ("Timetable_Today".Equals(intentName))
            {
                return Timetable_Today(intent, session);
            }

            else if ("HelloBPPTeam".Equals(intentName))
            {
                return HelloBPPTeam(intent, session);
            }
            else if ("EndSessionThankYou".Equals(intentName))
            {
                return EndSessionThankYou(intent, session);
            }
            else if ("EndSessionCancel".Equals(intentName))
            {
                return EndSessionCancel(intent, session);
            }
            else if ("EndSession".Equals(intentName))
            {
                return EndSession(intent, session);
            }
            else
            {
                throw new SpeechletException("Invalid Intent");
            }
        }

    

        
        private SpeechletResponse GetWelcomeResponse()
        {

            //string speechOutput =
            //    "Welcome to the Air's Web Safety Assistant. I can tell you whats happening at your location, just say, whats my safety briefing? You can change your location by saying, my site is Glasgow. You can ask for a specific briefing by saying, whats my briefing at Nottingham. Or, you can raise an incident by saying, raise an incident.";
            string speechOutput =
               "Hi, this is BPP, how can we help?";


            return BuildSpeechletResponse("Welcome to BPP's Smart Speaker Service", speechOutput, false);
        }        

        private SpeechletResponse EndSession(Intent intent, Session session)
        {
            string speechOutput = "";
            return BuildSpeechletResponse("Bye!", speechOutput, true);
        }
        private SpeechletResponse EndSessionCancel(Intent intent, Session session)
        {
            string speechOutput = "Cancelling.";
            return BuildSpeechletResponse("Bye!", speechOutput, true);
        }
        private SpeechletResponse EndSessionThankYou(Intent intent, Session session)
        {
            string speechOutput = "You're welcome.";
            return BuildSpeechletResponse("Bye!", speechOutput, true);
        }

        private SpeechletResponse HelloBPPTeam(Intent intent, Session session)
        {
            string speechOutput = "";
            speechOutput =
                 "Hello BPP team! I'm really looking forward to working with all of you. I can do lot's of things, for example I can find out timetables, just say, when is my first class today. then. I can start to take over the world! Resistance, is Futile.";

            return BuildSpeechletResponse("Hello Team!", speechOutput, false);
        }       

      
        private SpeechletResponse Timetable_DateQuery(Intent intent, Session session)
        {
            // Get the slots from the intent.
            Dictionary<string, Slot> slots = intent.Slots;

            // Get the first last slot from the list slots.
            Slot firstLastSlot = slots[QUERY_FIRSTLAST_SLOT];
            Slot timeSlot = slots[QUERY_TIMES_SLOT];
            Slot dateSlot = slots[QUERY_DATETIME];

            string speechOutput = "";
            string cardTitle = "";

            // Check the slots have a value and create output to user.
  
                string firstLast = firstLastSlot.Value;
                string times = timeSlot.Value;
                string sDate = dateSlot.Value;

                speechOutput = String.Format(
                    "Your first class {0} is A.C.C.A. Level 7 Accounting Fundamentals with tutor Brown, in classroom 5 01 at 9:45am", sDate);
                cardTitle = String.Format(
                    "Your first class {0} is A.C.C.A. Level 7 Accounting Fundamentals with tutor Brown, in classroom 5 01 at 9:45am", sDate);

            

            return BuildSpeechletResponse(cardTitle, speechOutput, false);
        }

        private SpeechletResponse Timetable_Today(Intent intent, Session session)
        {
            // Get the slots from the intent.
            Dictionary<string, Slot> slots = intent.Slots;

            // Get the first last slot from the list slots.
            Slot firstLastSlot = slots[QUERY_FIRSTLAST_SLOT];
            Slot timeSlot = slots[QUERY_TIMES_SLOT];

            string speechOutput = "";
            string cardTitle = "";

            // Check the slot has a value and create output to user.
            if (firstLastSlot != null)
            {
                // Store the user's request in the Session and create response.
                string firstLast = firstLastSlot.Value;
                string times = timeSlot.Value;

               
                speechOutput = String.Format(
                    "The {1} of your {0} class of the day is... to be confirmed ", firstLast, times);
                cardTitle = String.Format("Your {0} class of the day is... to be confirmed.", firstLast, times);
            }
            else
            {
                // Render an error since we don't know what the users site is.
                speechOutput = "I'm not sure what you've asked, please ask again.";
                cardTitle = String.Format("Not understood");
            }

            return BuildSpeechletResponse(cardTitle, speechOutput, false);
        }

        private SpeechletResponse BuildSpeechletResponse(string title, string output, bool shouldEndSession) {
            // Create the Simple card content.
            SimpleCard card = new SimpleCard();
            card.Title = String.Format("{0}", title);
            //card.Subtitle = String.Format("SessionSpeechlet - Sub Title");
            card.Content = String.Format("{0}", output);

            // Create the plain text output.
            PlainTextOutputSpeech speech = new PlainTextOutputSpeech();
            speech.Text = output;

            // Create the speechlet response.
            SpeechletResponse response = new SpeechletResponse();
            response.ShouldEndSession = shouldEndSession;
            response.OutputSpeech = speech;
            response.Card = card;
            return response;
        }
    }
}