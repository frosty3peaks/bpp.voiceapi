using System;
using AlexaSkillsKit.Speechlet;
using AlexaSkillsKit.Slu;
using AlexaSkillsKit.UI;
using System.Diagnostics;
using System.Linq;
using Sample.WebAPI.Services.Data;
using System.Text;

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
            if (intentName.Equals("ClassroomQueryIntent"))
            {
                return ClassroomQuery(intent, session);
            }
            else if (intentName.Equals("FAQIntent"))
            {
                return FAQIntent(intent, session);
            }
            else if (intentName.Equals("WIFIFAQIntent"))
            {
                return WIFIFAQIntent(intent, session);
            }
            else if (intentName.Equals("TrainsIntent"))
            {
                return TrainsIntent(intent, session);
            }
            else if (intentName.Equals("NearestTrainsIntent"))
            {
                return NearestTrainsIntent(intent, session);
            }
            else if ("Timetable_DateQuery".Equals(intentName))
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
            string speechOutput = "Welcome to BPP. How can I help?";

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

        private SpeechletResponse ClassroomQuery(Intent intent, Session session)
        {

            var slots = intent.Slots;
            
            var classType = slots["class"];
            
            if(classType.Value == null)
            {
                Debug.WriteLine("NULL");

                return BuildSpeechletResponse("Unknown Class", "Sorry, I did not recognise your class", false);
            }

            var resolution = classType.Resolutions.ResolutionsPerAuthority.FirstOrDefault();
            var status = resolution.Status.Code;
            var resolutionValue = resolution.Values.FirstOrDefault().Value;

            Debug.WriteLine($"**** {classType.Value} / {resolutionValue.Id} / {resolutionValue.Name}  ****");

            if (status.Equals("ER_SUCCESS_NO_MATCH",StringComparison.InvariantCultureIgnoreCase))
            {
                Debug.WriteLine("Unmatched");

                return BuildSpeechletResponse("Unmatched Class", "Sorry, I could not find your class", false);
            }

            var query = new VoiceQuery();

            var courses = query.GetCoursesForBody(resolutionValue.Id);

            var speechOutput = new StringBuilder();
            if (courses.Count == 0)
            {
                speechOutput.Append($"Could not find location for {classType.Value} class!");
            }
            else if (courses.Count == 1)
            {
                var datePart = courses[0].StartTime.ToShortDateString();
                var timePart = courses[0].StartTime.ToLocalTime();
                speechOutput.Append($"Your {resolutionValue.Name} class is in room {courses[0].Room} {dateOrTimePart(courses[0].StartTime)}");
            }
            else
            {
                speechOutput.Append($"Found {courses.Count} classes for {resolutionValue.Name}! ");
                foreach (var course in courses) {
                    speechOutput.Append($"{course.Paper} in room {courses[0].Room} {dateOrTimePart(courses[0].StartTime)}. ");
                }
            }

            return BuildSpeechletResponse(classType.Value, speechOutput.ToString(), false);
        }

        private string dateOrTimePart(DateTime startDateTime)
        {
            var datePart = startDateTime.ToShortDateString();
            var timePart = startDateTime.Hour<=12 ? startDateTime.Hour : startDateTime.Hour-12;
            if (datePart == DateTime.Now.ToShortDateString())
            {
                return $"at {timePart} O Clock!";
            }
            else
            {
                return $"on {datePart} at {timePart} O Clock!";
            }
        }

        private SpeechletResponse FAQIntent(Intent intent, Session session)
        {
            var speechOutput = $"You can get IT support by calling 03300603850, or visit the VLE online to access the IT self service portal";

            return BuildSpeechletResponse("IT Support request", speechOutput, false);
        }

        private SpeechletResponse WIFIFAQIntent(Intent intent, Session session)
        {
            var speechOutput = $"You can connect to the BPP student WIFI using passwords provided within the classrooms";

            return BuildSpeechletResponse("Student WiFi", speechOutput, false);
        }

        private SpeechletResponse TrainsIntent(Intent intent, Session session)
        {
            var slots = intent.Slots;

            var stationName = slots["station"];

            if (stationName.Value == null)
            {
                Debug.WriteLine("NULL");

                return BuildSpeechletResponse("Unknown station Name", "Sorry, I did not recognise your station", false);
            }

            var resolution = stationName.Resolutions.ResolutionsPerAuthority.FirstOrDefault();
            var status = resolution.Status.Code;
            var resolutionValue = resolution.Values.FirstOrDefault().Value;

            Debug.WriteLine($"**** {stationName.Value} / {resolutionValue.Id} / {resolutionValue.Name}  ****");

            if (status.Equals("ER_SUCCESS_NO_MATCH", StringComparison.InvariantCultureIgnoreCase))
            {
                Debug.WriteLine("Unmatched");

                return BuildSpeechletResponse("Unmatched Station", "Sorry, I could not find your station", false);
            }
            if (resolutionValue.Id == "victoria")
            {
                return BuildSpeechletResponse("Train Station Directions", "Victoria Station is about 20 minutes walking, may I suggest you get a taxi", false);
            }
            else if (resolutionValue.Id == "piccadilly")
            {
                return BuildSpeechletResponse("Train Station Directions", "Piccadilly Station is about 10 minutes walking, or you could get a taxi", false);
            }
            else if (resolutionValue.Id == "oxfordroad" || resolutionValue.Id == "oxford")
            {
                return BuildSpeechletResponse("Train Station Directions", "Oxford Road Station is about 5 minutes walking.", false);
            }
            return BuildSpeechletResponse("Unknown station Name", "Sorry, I did not recognise your station", false);
        }

        private SpeechletResponse NearestTrainsIntent(Intent intent, Session session)
        {
            return BuildSpeechletResponse("Train Station Directions", "The nearest station is Oxford Road.", false);
        }

        private SpeechletResponse Timetable_DateQuery(Intent intent, Session session)
        {
            // Get the slots from the intent.
            var slots = intent.Slots;

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
           var slots = intent.Slots;

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