using CSCAssignment.Task2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CSCAssignment.Task2.Controllers
{
    public class ItemController : ApiController
    {
        List<Item> items = new List<Item>
        {
            new Item { Id = 1, Name = "Chicken", Price = 4.00 },

            new Item { Id = 2, Name = "Pork", Price = 6.00 },

            new Item { Id = 3, Name = "Fish", Price = 10.00 }
        };

        [HttpGet]
        [Route("api/t2/item")]
        public IEnumerable<Item> GetAllItems()
        {
            return items;
        }

        [HttpPost]
        [Route("api/t2/item")]
        public HttpResponseMessage PostNewItem(Item item)
        {

            if (ModelState.IsValid)
            {
                int current_largest_id = -1;
                foreach(Item i in items)
                {
                    if(i.Id > current_largest_id)
                    {
                        current_largest_id = i.Id;
                    }
                }
                item.Id = current_largest_id + 1;
                items.Add(item);
                return Request.CreateResponse(HttpStatusCode.Created, items);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        [HttpPut]
        [Route("api/t2/item/{id:int}")]
        public HttpResponseMessage PutItem(int id, Item updatedItem)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            updatedItem.Id = id;
            bool itemFound = false;
            for(int idx=0; idx < items.Count; idx++)
            {
                if (items[idx].Id.Equals(id))
                {
                    itemFound = true;
                    items[idx] = updatedItem;
                }
            }

            if (itemFound)
            {
                return Request.CreateResponse(HttpStatusCode.OK, items);
            }
            else { 
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, String.Format("Item with id {0} not found",id.ToString()));
            }
        }

        [HttpDelete]
        [Route("api/t2/item/{id:int}")]
        public HttpResponseMessage DeleteItem(int id)
        {
            bool itemFound = false;
            for (int idx = 0; idx < items.Count; idx++)
            {
                if (items[idx].Id.Equals(id))
                {
                    itemFound = true;
                    items.Remove(items[idx]);
                }
            }

            if (itemFound)
            {
                return Request.CreateResponse(HttpStatusCode.OK, items);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, String.Format("Item with id {0} not found", id.ToString()));
            }
        }

    }
}
