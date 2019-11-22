using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Configuration;
using System.IO;

namespace Airsweb.VoiceAPI.Services
{
	public class airVizService
	{
		public string GetColorCode(string input)
        {
			switch (input.ToLower()) 
				{
                case "red":
                    return "255-000-000";

                case "green":
                    return "000-255-000";

                case "blue":
                    return "000-000-255";
 
                case "purple":
                    return "100-000-255";
          
                case "turquoise":
                    return "000-255-180";
      
                case "yellow":
                    return "127-255-000";
               
                case "amber":
                    return "127-255-000";
           
                case "white":
                    return "255-255-255";

                case "off":
                    return "000-000-000";

                default:
                    return "000-000-000";
            }
			
        }
        public void SetJacket(string colorcode, string flashing)
        {
            var _req = (HttpWebRequest)WebRequest.Create(ConfigurationManager.AppSettings["airVizJacketAPIURL"] + "setlocation/0/2/" + GetColorCode(colorcode) + "-" + flashing + "/nothing,");
            _req.ContentType = "text/html";
            _req.Method = "POST";
            _req.ContentLength = 0;
            //using (var streamWriter = new StreamWriter(_req.GetRequestStream()))
            //{
            //    string json = content;

            //    streamWriter.Write(json);
            //    streamWriter.Flush();
            //    streamWriter.Close();
            //}

            var _resp = (HttpWebResponse)_req.GetResponse();
            

        }
    }
}