using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

public partial class Samples_SimpleMapWithBubble : System.Web.UI.Page
{
    List<GooglePoint> gp = new List<GooglePoint>();
    protected void Page_Load(object sender, EventArgs e)
    {
        GoogleMapForASPNet1.PushpinClicked += new GoogleMapForASPNet.PushpinClickedHandler(OnPushpinClicked);
        if (!IsPostBack)
        {
            //You must specify Google Map API Key for this component. You can obtain this key from http://code.google.com/apis/maps/signup.html
            //For samples to run properly, set GoogleAPIKey in Web.Config file.
            GoogleMapForASPNet1.GoogleMapObject.APIKey = ConfigurationManager.AppSettings["GoogleAPIKey"];
            //GoogleMapForASPNet1.GoogleMapObject.APIKey = "3.6";

            //Specify width and height for map. You can specify either in pixels or in percentage relative to it's container.
            GoogleMapForASPNet1.GoogleMapObject.Width = "800px"; // You can also specify percentage(e.g. 80%) here
            GoogleMapForASPNet1.GoogleMapObject.Height = "600px";

            //Specify initial Zoom level.
            GoogleMapForASPNet1.GoogleMapObject.ZoomLevel = 16;
            //ITB -6.892943, 107.610170 
            //Alun2 -6.921912, 107.606910
            //Specify Center Point for map. Map will be centered on this point.
            GoogleMapForASPNet1.GoogleMapObject.CenterPoint = new GooglePoint("1", -6.921912, 107.606910);

            //Add pushpins for map. 
            //This should be done with intialization of GooglePoint class. 
            //ID is to identify a pushpin. It must be unique for each pin. Type is string.
            //Other properties latitude and longitude.
            string path = @"E:\Alun2.txt";
            // This text is added only once to the file.
            string[] file = File.ReadAllLines(path);

            int count = 0;
            while (count < file.Length)
            {
                string[] posisi = file[count].Split(',');
                gp.Add(new GooglePoint());
                Double la, lo;
                la = (Double.Parse(posisi[0], System.Globalization.NumberStyles.Float)); // 10000000000000000;
                lo = (Double.Parse(posisi[1], System.Globalization.NumberStyles.Float)); // 10000000000000;
                gp[count].ID = (count + 1).ToString();
                gp[count].Latitude = (double)la;
                gp[count].Longitude = (double)lo;
                gp[count].InfoHTML = "This is point " + (count + 1);
                GoogleMapForASPNet1.GoogleMapObject.Points.Add(gp[count]);
                count++;
            }
        }
    }

    void OnPushpinClicked(string pID)
    {
        //pID is ID of pushpin
        int node = Convert.ToInt32(pID);
        //double dLat = GoogleMapForASPNet1.GoogleMapObject.Points[pID].Latitude;
        //double dLng = GoogleMapForASPNet1.GoogleMapObject.Points[pID].Longitude;
        string path = @"E:\Clicked.txt";
        if (!File.Exists(path))
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.WriteLine(node.ToString());
            }
        }
        else
        {
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(node.ToString());
            }
        }
    }
}
