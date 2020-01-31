using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ParserBooking
{
    class URLFormatter
    {
        public URLFormatter(string checkin, string checkout, string destination)
        {
            BaseURL = "https://www.booking.com";
            SearchResult = "/searchresults.ru.html?aid=397643&label=yan104jc-1FCAEoggI46AdIM1gDaMIBiAEBmAEhuAEXyAEM2AEB6AEB-AEMiAIBqAIDuAKv36HxBcACAQ&sid=90a68cf28c5926987d30317b2eab0f3d&tmpl=searchresults&ac_click_type=b&ac_langcode=ru&ac_position=0&";
            Date = $"checkin_month={Convert.ToInt16(checkin.Split('.')[1])}&checkin_monthday={Convert.ToInt16(checkin.Split('.')[0])}&checkin_year={checkin.Split('.')[2]}" +
                $"&checkout_month={Convert.ToInt16(checkout.Split('.')[1])}&checkout_monthday={Convert.ToInt16(checkout.Split('.')[0])}&checkout_year={checkout.Split('.')[2]}";
            Other = "&class_interval=1&dest_type=city&dtdisc=0&from_sf=1&group_adults=1&group_children=0&inac=0&index_postcard=0&label_click=undef&no_rooms=1&offset=0&postcard=0&raw_dest_type=city&room1=A&sb_price_type=total&search_pageview_id=67376f979db90052&search_selected=1&shw_aparth=1&slp_r_match=0&src=index&src_elem=sb&srpvid=b0fb6ff6f42d00e6&";
            Destination = $"&ss={HttpUtility.HtmlEncode(destination)}&ss_all=0&ssb=empty&sshis=0&top_ufis=1&no_dorms=1";
            
        }
        public string URL
        {
            get
            {
                return BaseURL + SearchResult + Date + Other + Destination;
            }
        }
        public string BaseURL
        {
            get;
            private set;
        }
        string SearchResult { get; set; }
        string Date
        {
            get;
            set;
        }
        string Other
        {
            get;
            set;
        }
        string Destination
        {
            get;
            set;
        }
    }
}
