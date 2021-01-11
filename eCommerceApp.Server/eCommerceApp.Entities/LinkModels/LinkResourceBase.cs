using System.Collections.Generic;

namespace eCommerceApp.Entities.LinkModels
{
    public class LinkResourceBase
    {
        public LinkResourceBase() { }
        public List<Link> Links { get; set; } = new List<Link>();
    }
}