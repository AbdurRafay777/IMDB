using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMDB.Models
{
    public class Movie
    {
        public int m_ID { get; set; }
        public string m_name { get; set; }
        public string m_rating { get; set; }
        public string m_description { get; set; }
        public string m_releaseDate { get; set; }
        public string m_poster { get; set; }
        public string m_trailer { get; set; }
    }
}