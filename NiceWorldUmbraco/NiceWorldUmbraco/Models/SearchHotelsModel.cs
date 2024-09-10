using System.Collections.Generic;
using Umbraco.Core.Models.PublishedContent;
using Umbraco.Web.Models;

namespace NiceWorldUmbraco.Models
{
	public class SearchHotelsModel : ContentModel
	{
		public SearchHotelsModel(IPublishedContent content) : base(content)
		{
			Data = new List<KSItem>();
		}
		public List<KSItem> Data { get; set; }
	}

	public class KSItem
	{
		public string HotelName { get; set; }
		public string HotelDescription { get; set; }
		public string ImgUrl { get; set; }
		public string Url { get; set; }
		public string Address { get; set; }
		public List<string> Features { get; set; }
		public string MapPhotoUrl { get; set; }
		public List<string> ImgUrls { get; set; }
	}
}
