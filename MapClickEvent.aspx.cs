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
using System.Collections.Generic;
using System.IO;

public partial class Samples_MapClickEvent : System.Web.UI.Page
{
    public static List<List<double>> list_tuple = new List<List<double>>();
    protected void Page_Load(object sender, EventArgs e)
    {
        //Add event handler for PushpinMoved event
        GoogleMapForASPNet1.MapClicked += new GoogleMapForASPNet.MapClickedHandler(OnMapClicked);
        if (!IsPostBack)
        {

            //You must specify Google Map API Key for this component. You can obtain this key from http://code.google.com/apis/maps/signup.html
            //For samples to run properly, set GoogleAPIKey in Web.Config file.
            GoogleMapForASPNet1.GoogleMapObject.APIKey = ConfigurationManager.AppSettings["GoogleAPIKey"];

            //Specify width and height for map. You can specify either in pixels or in percentage relative to it's container.
            GoogleMapForASPNet1.GoogleMapObject.Width = "1000px"; // You can also specify percentage(e.g. 80%) here
            GoogleMapForASPNet1.GoogleMapObject.Height = "500px";

            //Specify initial Zoom level.
            GoogleMapForASPNet1.GoogleMapObject.ZoomLevel = 20;

            //Specify Center Point for map. Map will be centered on this point.
            GoogleMapForASPNet1.GoogleMapObject.CenterPoint = new GooglePoint("1", -6.892943, 107.610170);

        }
    }

     //Add event handler for Map Click event
    void OnMapClicked(double dLat, double dLng)
    {
        //Print clicked map positions
        lblPushpin1.Text = "(" + dLat.ToString() + "," + dLng.ToString() + ")";
        // This text is added only once to the file.
        string path = @"E:\ITB.txt";
        if (!File.Exists(path))
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(path))
            {

            }
        }
        using (StreamWriter sw = File.AppendText(path))
        {
            sw.WriteLine(dLat.ToString() + "," +dLng.ToString());
        }

        //Generate new id for google point
        string sID = "Point1";
        GooglePoint GP1 = new GooglePoint(sID, dLat, dLng);
        GoogleMapForASPNet1.GoogleMapObject.Points.Add(GP1);
    }
}
