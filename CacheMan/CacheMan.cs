﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

public static class CacheMan
{
    public static string Query(string urlString, string cacheKey = "cachedUntil")
    {
        string url = urlString;
        string fileNameUrlString = removeInvalidPathCharacters(urlString);

        XmlDocument x = new XmlDocument();
        if (File.Exists(fileNameUrlString))
        {
            x.Load(fileNameUrlString);
        }

        var cacheTime = x.SelectSingleNode(string.Format("//{0}" , cacheKey ));
        if (cacheTime != null)
        {
            DateTime ou;
            if (DateTime.TryParse(cacheTime.InnerText, out ou) && ou < DateTime.Now)
            {
                System.Diagnostics.Debug.WriteLine("cache out of date.");
                x.LoadXml(performQuery(url));
                x.Save(fileNameUrlString);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("using old data.");
                x.Load(fileNameUrlString);
            }
            return x.InnerXml;
        }
        else
        {
            //we never retrieved this before.
            System.Diagnostics.Debug.WriteLine("cache doesn't exist.");
            x.LoadXml(performQuery(url));
            x.Save(fileNameUrlString);
        }
            
        return x.InnerXml;
    }

    private static string removeInvalidPathCharacters(string urlString)
    {
        string toReturn = urlString;
        foreach(char item in Path.GetInvalidFileNameChars())
        {
            toReturn = toReturn.Replace(item, '_');
        }
        return toReturn;
    }

    private static string performQuery(string urlString)
    {
        string toReturn = "No Response";
        HttpWebRequest request = HttpWebRequest.Create(urlString) as HttpWebRequest;
        WebResponse response = request.GetResponse();
        using(System.IO.StreamReader strea = new System.IO.StreamReader(response.GetResponseStream()))
        {
            toReturn = strea.ReadToEnd();
        }
        response.Close();
            
        return toReturn;
    }
}