using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NiceWorldUmbraco.Models;
using StackExchange.Profiling.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.ModelsBuilder;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace NiceWorldUmbraco.Controllers.Render
{
	public class SearchHotelsController : RenderMvcController
	{
		public ActionResult Index(ContentModel model, string query)
		{
			// Receive home page Id from umbraco console
			const int homePageId = 1088;
			var homeContent = UmbracoContext.Content.GetById(homePageId);
			if (homeContent == null)
			{
				return CurrentTemplate(model);
			}
			var ksItems = new List<KSItem>();

			// Get list of available cities
			var listOfCity = ((Umbraco.Core.Models.PublishedContent.PublishedContentWrapped)homeContent).ChildrenOfType("City").ToList();
			foreach (var city in listOfCity)
			{
				// Get list of available districts in particular city
				var listOfDistrict = city.ChildrenOfType("District").ToList();
				foreach (var district in listOfDistrict)
				{
					// Get list of available hotel in particular district
					var listOfHotel = district.ChildrenOfType("Hotel").ToList();

					foreach (var hotel in listOfHotel)
					{
						// Get values in hotel
						var htlName = hotel.Value<string>("HsdTitle");
						var htlUrl = ((Umbraco.Core.Models.PublishedContent.PublishedContentWrapped)hotel).Url(); 
						var htlDescription = hotel.Value<string>("HsdDescription");
						var htlPictures = hotel.Value<List<Umbraco.Core.Models.PublishedContent.IPublishedContent>>("HsdPicture");
						var htlPresentPicture = ((Umbraco.Core.Models.PublishedContent.PublishedContentWrapped)htlPictures.FirstOrDefault()).Url();
						var listOfPictures = hotel.Value<List<Umbraco.Core.Models.PublishedContent.IPublishedContent>>("HsdPicture").ToList();
						var mapPhoto = hotel.Value<Umbraco.Core.Models.PublishedContent.IPublishedContent>("MapPhoto");
						var mapPhotoUrl = mapPhoto.Url();
						var htlAddress = hotel.Value<string>("MapDescription");
						
						// TODO: We need ways to load Features component
						var htlFeatures = hotel.Value<object>("Features");
						var jsonSerializerSettings = new JsonSerializerSettings
						{
							ReferenceLoopHandling = ReferenceLoopHandling.Serialize,
							PreserveReferencesHandling = PreserveReferencesHandling.Objects,
						};

						var jsonStringFeatures = JsonConvert.SerializeObject(htlFeatures, jsonSerializerSettings);
						var features = JsonConvert.DeserializeObject<List<FeaturesModel>>(jsonStringFeatures);
						List<string> ImgUrls = new List<string>();
						foreach (var picture in listOfPictures)
						{
							ImgUrls.Add(picture.Url());
						}
						ksItems.Add(new KSItem()
						{
							HotelDescription = htlDescription,
							HotelName = htlName,
							Url = htlUrl,
							ImgUrl = htlPresentPicture,
							Address = htlAddress,
							Features = features,
							MapPhotoUrl = mapPhotoUrl,
							ImgUrls = ImgUrls
						});
					}
				}
			}
			var ksModel = new SearchHotelsModel(model.Content)
			{
				Data = ksItems
			};

			return CurrentTemplate(ksModel);
		}
	}
}