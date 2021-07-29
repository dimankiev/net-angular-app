using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace OrdersPortal.Controllers
{
    [ApiController]
    [Route("api/")]
    public class OrdersController : ControllerBase
    {
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(ILogger<OrdersController> logger)
        {
            _logger = logger;
        }
        
        [HttpGet]
        [Route("orders/get")]
        public async Task<IEnumerable<Order>> GetOrders()
        {
            return await new Database().GetAllOrders(new Order());
        }

        [HttpPost]
        [Route("orders/add")]
        public async Task<PortalResponse> AddOrder([FromBody] Order newOrder)
        {
            PortalResponse res = new PortalResponse();
            Customer orderCustomer;
            List<Product> orderProducts = new();
            // Data cleanup
            newOrder.CustomerName = newOrder.CustomerName.Trim();
            newOrder.CustomerAddress = newOrder.CustomerAddress.Trim();
            try
            {
                List <Customer> Customers = await new Database().GetAllCustomers(
                    new Customer{Name = newOrder.CustomerName}
                    );
                orderCustomer = Customers.Single(customer => customer.Name == newOrder.CustomerName);
            }
            catch (Exception err)
            {
                res.Success = false;
                res.StatusCode = 404;
                res.Message = "CustomerNotFound";
                return res;
            }

            int totalCost = 0;
            try
            {
                foreach (var ordered in newOrder.Products)
                {
                    List<StockProduct> products = await new Database().GetAllProducts(new StockProduct());
                    orderProducts.Add(products.Single(stockProduct =>
                            {
                                bool check = stockProduct.ProductId == ordered.ProductId && stockProduct.Stock >= ordered.Quantity;
                                if (check) totalCost += ordered.Quantity * stockProduct.Price;
                                return check;
                            }
                        )
                    );
                    if (newOrder.Products.Length > orderProducts.Count) 
                        throw new Exception("NotAllProductsNotAvailable");
                }
            }
            catch (Exception err)
            {
                res.Success = false;
                res.StatusCode = 404;
                res.Message = "ProductNotAvailable";
                return res;
            }

            newOrder.Number = new Random().Next(0, int.MaxValue);
            new Database().InsertOrder(newOrder);
            orderCustomer.TotalOrders += 1;
            orderCustomer.TotalCost += totalCost;
            res.Success = true;
            res.StatusCode = 200;
            res.Message = "Success";
            return res;
        }
        
        [HttpGet]
        [Route("products/get")]
        public async Task<IEnumerable<StockProduct>> GetProducts()
        {
            return await new Database().GetAllProducts(new StockProduct());
        }

        [HttpPost]
        [Route("products/add")]
        public async Task<PortalResponse> AddProduct([FromBody] StockProduct newProduct)
        {
            PortalResponse res = new PortalResponse();
            StockProduct existingProduct;
            try
            {
                List<StockProduct> products = await new Database().GetAllProducts(new StockProduct
                {
                    Name = newProduct.Name
                });
                existingProduct = products.Single(product => product.Name == newProduct.Name);
                res.Success = false;
                res.StatusCode = 404;
                res.Message = "ProductAlreadyExists";
                return res;
            } catch { }

            newProduct.ProductId = new Random().Next(0, int.MaxValue);
            newProduct.Name = newProduct.Name.Trim();
            new Database().InsertProduct(newProduct);
            res.Success = true;
            res.StatusCode = 200;
            res.Message = "Success";
            return res;
        }
        
        [HttpPost]
        [Route("products/delete")]
        public async Task<PortalResponse> DeleteProduct([FromBody] StockProduct product)
        {
            PortalResponse res = new PortalResponse();
            StockProduct existingProduct;
            try
            {
                List<StockProduct> products = await new Database().GetAllProducts(new StockProduct
                {
                    ProductId = product.ProductId
                });
                existingProduct = products.Single(el => el.ProductId == product.ProductId);
            }
            catch
            {
                res.Success = false;
                res.StatusCode = 404;
                res.Message = "ProductDoesNotExist";
                return res;
            }
            
            new Database().DeleteProduct(product);
            res.Success = true;
            res.StatusCode = 200;
            res.Message = "Success";
            return res;
        }
        
        [HttpGet]
        [Route("customers/get")]
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await new Database().GetAllCustomers(new Customer());
        }

        [HttpPost]
        [Route("customers/add")]
        public async Task<PortalResponse> AddCustomer([FromBody] Customer newCustomer)
        {
            PortalResponse res = new PortalResponse();
            Customer existingCustomer;
            try
            {
                newCustomer.Name = newCustomer.Name.Trim();
                newCustomer.Address = newCustomer.Address.Trim();
            }
            catch
            {
                res.Success = false;
                res.StatusCode = 500;
                res.Message = "InputDataError";
                return res;
            }
            try
            {
                List <Customer> customers = await new Database().GetAllCustomers(
                    new Customer{Name = newCustomer.Name, Address = newCustomer.Address}
                );
                existingCustomer = customers.Single(
                    customer => customer.Name == newCustomer.Name && customer.Address == newCustomer.Address
                                );
                res.Success = false;
                res.StatusCode = 404;
                res.Message = "CustomerAlreadyExists";
                return res;
            } catch { }

            newCustomer.TotalCost = 0;
            newCustomer.TotalOrders = 0;
            new Database().InsertCustomer(newCustomer);
            res.Success = true;
            res.StatusCode = 200;
            res.Message = "Success";
            return res;
        }
    }
}