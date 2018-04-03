using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

public partial class Samples_SimpleMapWithBubble : System.Web.UI.Page
{
    static List<GooglePoint> gp = new List<GooglePoint>();
    List<GooglePolyline> gpl = new List<GooglePolyline>();
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
            GoogleMapForASPNet1.GoogleMapObject.CenterPoint = new GooglePoint("1", -6.892943, 107.610170);

            //Add pushpins for map. 
            //This should be done with intialization of GooglePoint class. 
            //ID is to identify a pushpin. It must be unique for each pin. Type is string.
            //Other properties latitude and longitude.
            string path = @"E:\ITB.txt";
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
                gp[count].InfoHTML = "" + (count + 1);
                GoogleMapForASPNet1.GoogleMapObject.Points.Add(gp[count]);
                count++;
            }
            
            //path = @"E:\JarakLurusITB.txt";
            //using (StreamWriter sw = File.CreateText(path)) { }
            //for (int i = 0; i < count; i++)
            //{
            //    for (int j = 0; j < count; j++)
            //    {
            //        double jarak = Measure(gp[i].Latitude, gp[i].Longitude, gp[j].Latitude, gp[j].Longitude);
            //        using (StreamWriter sw = File.AppendText(path))
            //        {
            //            sw.WriteLine(gp[i].ID.ToString() + "$" + gp[j].ID.ToString() + "$" + jarak);
            //        }
            //    }
            //}
        }
    }

    void OnPushpinClicked(string pID)
    {
        //pID is ID of pushpin
        string node = pID;
        //double dLat = GoogleMapForASPNet1.GoogleMapObject.Points[pID].Latitude;
        //double dLng = GoogleMapForASPNet1.GoogleMapObject.Points[pID].Longitude;
        string path = @"E:\Clicked.txt";
        if (!File.Exists(path))
        {
            // Create a file to write to.
            using (StreamWriter sw = File.CreateText(path))
            {
                sw.Write(node.ToString());
            }
        }
        else
        {
            string node1 = File.ReadAllText(path);
            lblPushpin1.Text = "(" + node1 + ")";
            Astar(node1, node);
        }

    }

    double Measure(double lat1, double lon1, double lat2, double lon2)
    {  // generally used geo measurement function
        var R = 6378.137; // Radius of earth in KM
        var dLat = lat2 * Math.PI / 180 - lat1 * Math.PI / 180;
        var dLon = lon2 * Math.PI / 180 - lon1 * Math.PI / 180;
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
        Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
        Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        var d = R * c;
        return d * 1000; // meters
    }

    public void Astar(string awal, string akhir)
    {
        string path = @"E:\NodeITB.txt";
        string path_jarak = @"E:\JarakLurusITB.txt";
        string[] text = System.IO.File.ReadAllLines(path);
        string current = awal;
        double total = 0;
        Boolean finish = false;
        List<List<string>> data = new List<List<string>>();
        List<List<string>> list_jarak = new List<List<string>>();
        List<string> answer = new List<string>();

        List<List<double>> jarak = new List<List<double>>();
        List<List<string>> simpul = new List<List<string>>();
        string[] jrk = System.IO.File.ReadAllLines(path_jarak);
        int i = 0;
        foreach (string line in text)
        {
            List<string> temp = new List<string>();
            temp = line.Split(',').ToList();
            data.Add(temp);
        }
        foreach (string line in jrk)
        {
            List<string> temp = new List<string>();
            temp = line.Split('$').ToList();
            list_jarak.Add(temp);
        }
        int idx;
        foreach (List<string> line in data)
        {
            if (line.Exists(e => e == current) && line[0] != current)
            {
                idx = list_jarak.FindIndex(lst => lst[0] == current && lst[1] == line[0]);
                List<double> temp = new List<double>();
                List<string> temp_str = new List<string>();
                total = double.Parse(list_jarak[idx][1]);
                idx = list_jarak.FindIndex(lst => lst[0] == line[0] && lst[1] == akhir);
                total += double.Parse(list_jarak[idx][1]);
                temp.Add(total);
                temp.Add(i);
                temp.Add(double.Parse(list_jarak[idx][1]));
                jarak.Add(temp);
                temp_str.Add(line[0]);
                simpul.Add(temp_str);
                i++;
            }
        }
        List<string> ans = new List<string>();
        int j = 0;
        double jarak_sblm = 0;
        while (!finish)
        {
            if (current != akhir)
            {
                jarak = jarak.OrderBy(lst => lst[0]).ToList();
                j = 0;
                foreach (List<double> nilai in jarak)
                {

                    if (nilai[0] != 0)
                    {
                        current = simpul[(int)jarak[j][1]].Last();
                        jarak_sblm = jarak[j][2];
                        ans = simpul[(int)jarak[j][1]].GetRange(0, simpul[(int)jarak[j][1]].Count);
                        jarak.Remove(nilai);
                        break;
                    }
                    j++;
                }

                foreach (List<string> line in data)
                {
                    if (line.Exists(e => e == current) && line[0] != current && !line.Exists(e => e == awal))
                    {
                        foreach (string jawab in ans)
                        {
                            //foreach(distance in list_jarak)
                            //tambahin jarak yg sblmnya sblmnya disini
                            if (line.Exists(e => e == jawab))
                            {
                                break;
                            }
                        }
                        idx = list_jarak.FindIndex(lst => lst[0] == current && lst[1] == line[0]);
                        List<double> temp = new List<double>();
                        List<string> temp_str = new List<string>();
                        total = double.Parse(list_jarak[idx][2]) + jarak_sblm;
                        idx = list_jarak.FindIndex(lst => lst[0] == line[0] && lst[1] == akhir);
                        temp.Add(total+ double.Parse(list_jarak[idx][1]));
                        temp.Add(i);
                        temp.Add(total);
                        foreach (string node in ans)
                        {
                            temp_str.Add(node);
                        }
                        temp_str.Add(line[0]);
                        jarak.Add(temp);
                        simpul.Add(temp_str);
                        i++;
                    }
                }
            }
            else
            {
                finish = true;
            }
        }
        Label2.Text = awal;
        int ct = 0;
        string b4 = awal;
        foreach (string jawaban in ans)
        {
            gpl.Add(new GooglePolyline());
            gpl[ct].ID = "Poly" + ct.ToString();
            gpl[ct].Points.Add(gp[Convert.ToInt32(jawaban)-1]);
            gpl[ct].Points.Add(gp[Convert.ToInt32(b4)-1]);
            gpl[ct].ColorCode = "#00ffff";
            gpl[ct].Width = 5;
            b4 = jawaban;
            GoogleMapForASPNet1.GoogleMapObject.Polylines.Add(gpl[ct]);
            ct++;
            Label2.Text += " " + jawaban;
        }
        lblPushpin1.Text = total.ToString();
        //List<GooglePoint> delete = new List<GooglePoint>();
        foreach (GooglePoint poin in gp)
        {
            if (!ans.Contains(poin.ID) && poin.ID != awal)
            {
                //delete.Add(poin);
                GoogleMapForASPNet1.GoogleMapObject.Points.Remove(poin.ID);
            }
        }
    }
}
