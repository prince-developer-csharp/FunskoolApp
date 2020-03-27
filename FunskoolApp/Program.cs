using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunskoolApp
{
    class Program
    {
        private static int categoryId;
        private static int userId;
        private static int orderId;
        private static char placeOrder;
        private static int shippingId;

        static void Main(string[] args)
        {
            int choice;
            bool loop = true;
            string adminName;
            string adminPassword;
            string verifiedAdmin;
            string userName;
            string password;
            while (loop)
            {
                Console.WriteLine("1.Admin");
                Console.WriteLine("2.SignIn");
                Console.WriteLine("3.Signup");
                Console.WriteLine("4.Show details by name");
                Console.WriteLine("5.Exit");
                Console.WriteLine("Enter your choice : ");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice)
                {
                    case 1:
                        Console.WriteLine("Enter the admin name:");
                        adminName = Console.ReadLine();
                        Console.WriteLine("Enter the admin password:");
                        adminPassword = Console.ReadLine();
                        checkAdmin(adminName, adminPassword);
                        break;

                    case 2:
                        Console.WriteLine("Enter the user name: ");
                        userName = Console.ReadLine();
                        Console.WriteLine("Enter the password: ");
                        password = Console.ReadLine();
                        checkUser(userName, password);
                        break;

                    case 3:
                        Console.WriteLine("Enter the user name: ");
                        userName = Console.ReadLine();
                        Console.WriteLine("Enter the password: ");
                        password = Console.ReadLine();
                        SignUp(userName, password);
                        break;

                    case 4:
                        showOrder();
                        break;

                    case 5:
                        loop = false;
                        break;
                }
            } 
        }

        private static void showOrder()
        {
            var dbContext = new FunskoolDbEntities();
            Console.WriteLine("Enter the name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Your order details : ");
            var OrderList = dbContext.vGetOrderDetails.ToList().Where(t => t.name == name);
            foreach (var list in OrderList)
            {
                Console.WriteLine("Name : " + list.name + " Toy name : " + list.ToyName + " Price : " + list.Price + " OrderDetailId :" + list.OrderDetail);
            }
        }

        private static void checkUser(string userName, string password)
        {
            var dbContext = new FunskoolDbEntities();
            var user = dbContext.users.Where(t => t.name == userName && t.password == password).FirstOrDefault();
            if (user == null)
            {
                Console.WriteLine("invalid credentials");
            }
            else
            {
                Console.WriteLine("Successfully logged in");
                userId = user.UserId;
                userFunction();
            }
        }

        private static void SignUp(string userName, string password)
        {
            var dbContext = new FunskoolDbEntities();
            var user = new user()
            {
                name = userName,
                password = password
            };
            try
            {
                dbContext.users.Add(user);
                dbContext.SaveChanges();
                Console.WriteLine("Users added");
                var customer = dbContext.users.Where(t => t.name == userName).FirstOrDefault();
                userId = customer.UserId;
                userFunction();
            }catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void userFunction()
        {
            int categoryID = showCategory();
            var dbContext = new FunskoolDbEntities();

            //add userid in order table

            var order = new Order()
            {
                UserId = userId
            };

            try
            {
                dbContext.Orders.Add(order);
                dbContext.SaveChanges();

                var Singleorder = dbContext.Orders.Where(t => t.UserId == userId).FirstOrDefault();
                orderId = Singleorder.OrderId;
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
           
            var toyList = dbContext.Toys.ToList().Where(t => t.CategoryId == categoryID);    

            char buyToy = 'y';
            while(buyToy == 'y')
            {
                foreach (var list in toyList)
                {
                    Console.WriteLine("Toy Id : " + list.ToyId + " Toy name : " + list.ToyName + " Toy Price : " + list.Price);
                }
                Console.WriteLine("Enter the toy id : ");
                int toyId = Convert.ToInt32(Console.ReadLine());

                var orderDetails = new orderDetail()
                {
                    toyid = toyId,
                    OrderId = orderId
                };

                try
                {
                    dbContext.orderDetails.Add(orderDetails);
                    dbContext.SaveChanges();
                    Console.WriteLine("order added");
                }catch(Exception e)
                {
                    Console.WriteLine(e);
                }

                Console.WriteLine("do u wanna add more order y/n");
                buyToy = Convert.ToChar(Console.ReadLine());
            }

            Console.WriteLine("Do you want to place order y/n");
            placeOrder = Convert.ToChar(Console.ReadLine());
            if(placeOrder == 'y')
            {
                shipping(orderId);
            }
            
        }

        private static void shipping(int orderId)
        {
            var dbContext = new FunskoolDbEntities();
            Console.WriteLine("Enter the full name : ");
            string fullName = Console.ReadLine();
            Console.WriteLine("Enter mobile number :");
            string mobileNumber = Console.ReadLine();
            Console.WriteLine("Enter the address");
            string address = Console.ReadLine();

            var shippingdetails = new ShippingOrder()
            {
                FullName = fullName,
                mobileNumber = mobileNumber,
                Address = address
            };

            try
            {
                dbContext.ShippingOrders.Add(shippingdetails);
                dbContext.SaveChanges();
                var singleShipping = dbContext.ShippingOrders.Where(t => t.mobileNumber == mobileNumber).FirstOrDefault();
                shippingId = singleShipping.ShippingId;

                var PlaceOrder = new PlaceOrder()
                {
                    ShippingId = shippingId,
                    OrderId = orderId
                };

                try
                {
                    dbContext.PlaceOrders.Add(PlaceOrder);
                    dbContext.SaveChanges();

                }catch(Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static int showCategory()
        {
            var dbContext = new FunskoolDbEntities();
            var categoryList = dbContext.ToyCategories;
            foreach(var list in categoryList)
            {
                Console.WriteLine(list.CategoryId +". "+"Category Name : " + list.ToyCategoryName);
            }

            Console.WriteLine("Enter the category id : ");
            categoryId = Convert.ToInt32(Console.ReadLine());
            return categoryId;
        }

        private static void checkAdmin(string adminName, string adminPassword)
        {

            var dbContext = new FunskoolDbEntities();
            var admin = dbContext.Admins.Where(t => t.name == adminName && t.password == adminPassword).FirstOrDefault();
            if (admin == null)
            {
                Console.WriteLine("invalid credentials");
            }
            else
            {
                Console.WriteLine("Successfully logged in");
                adminFunction();
            }
        }

        private static void adminFunction()
        {
            string categoryName;
            string locatedPlant;
            string state;
            int adminChoice;
            bool adminLoop = true;
            var category = new ToyCategory();
            while (adminLoop)
            {
                Console.WriteLine("1.Add manufacture plants");
                Console.WriteLine("2.Add Category");
                Console.WriteLine("3.Add Toys");
                Console.WriteLine("Enter your admin Choice : ");
                adminChoice = Convert.ToInt32(Console.ReadLine());
                switch(adminChoice)
                {
                    case 1:
                        Console.WriteLine("Enter the plant located : ");
                        locatedPlant = Console.ReadLine();
                        Console.WriteLine("Enter the state : ");
                        state = Console.ReadLine();
                        //method for adding manufacture plant
                        addManufacturePlant(locatedPlant, state);
                       
                        break;
                    case 2:
                        Console.WriteLine("Enter the category name:");
                        categoryName = Console.ReadLine();
                        category = new ToyCategory();
                        //method for adding category
                        addCategory(categoryName);
                        break;
                    case 3:
                        addToy();
                        Console.WriteLine("Enter the manufacture plant ID and category id : ");
                        int plantId = Convert.ToInt32(Console.ReadLine());
                        int categoryId = Convert.ToInt32(Console.ReadLine());
                        Console.WriteLine("Enter the toy name: ");
                        string name = Console.ReadLine();
                        Console.WriteLine("Enter the price of toy");
                        decimal price = Convert.ToDecimal(Console.ReadLine());
                        //method for adding toy
                        
                        break;
                    case 4:
                        adminLoop = false;
                        break;
                }
            }
        }

        private static void addToy()
        {
            var dbContext = new FunskoolDbEntities();
            var plantList = dbContext.ManufacturePlants;
            var categoryList = dbContext.ToyCategories;
            Console.WriteLine("Plant List");
            foreach(var plant in plantList)
            {
                Console.WriteLine(plant.PlantId + ". " + plant.LocatedPlant + " " + plant.State);
            }
            Console.WriteLine("Category List");
            foreach(var list in categoryList)
            {
                Console.WriteLine(list.CategoryId + ". " + list.ToyCategoryName);
            }
            Console.WriteLine("Enter the manufacture plant ID and category id : ");
            int plantId = Convert.ToInt32(Console.ReadLine());
            int categoryId = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Enter the toy name: ");
            string name = Console.ReadLine();
            Console.WriteLine("Enter the price of toy");
            decimal price = Convert.ToDecimal(Console.ReadLine());

            var toy = new Toy()
            {
                ToyName = name,
                Price = price,
                PlantId = plantId,
                CategoryId = categoryId
            };
            try
            {
                dbContext.Toys.Add(toy);
                dbContext.SaveChanges();
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void addCategory(string categoryName)
        {
            var dbContext = new FunskoolDbEntities();
            var Category = new ToyCategory()
            {
                ToyCategoryName = categoryName
            };

            try
            {
                dbContext.ToyCategories.Add(Category);
                dbContext.SaveChanges();
                Console.WriteLine("category added");
            }catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void addManufacturePlant(string locatedPlant, string state)
        {
            int companyId = 1;
            var dbContext = new FunskoolDbEntities();
            var plant = new ManufacturePlant()
            {
                LocatedPlant = locatedPlant,
                State = state,
                CompanyId = companyId
            };

            try
            {
                dbContext.ManufacturePlants.Add(plant);
                dbContext.SaveChanges();
                Console.WriteLine("plant added");
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
