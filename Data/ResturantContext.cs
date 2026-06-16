using Microsoft.EntityFrameworkCore;
using SaqrResturant.Models;

namespace SaqrResturant.Data
{
    public class ResturantContext : DbContext
    {
        public DbSet<deliveryModel> Deliveries { get; set; } = null!;
        public DbSet<UserModel> Users { get; set; } = null!;
        public DbSet<orderModel> Orders { get; set; } = null!;
        public DbSet<OrderDetailsModel> OrderDetails { get; set; } = null!;
        public DbSet<menuModel> Menu { get; set; } = null!;
        public DbSet<ContactUsModel> Inbox { get; set; } = null!; 



        public ResturantContext(DbContextOptions<ResturantContext> options)
        : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // this line is to make sure that on deletion of a user nothing will happen,
            // if omitted, the nuGet package manager will complain that there's a cylce foreign key reference
            // meaning that delivery table has FK from orders & users and orders has FK from users
            // which could cause a cylce if we delete a user, nuGet is just paranoid.
            modelBuilder.Entity<deliveryModel>()
                .HasOne(d => d.User)
                .WithMany()
                .HasForeignKey(d => d.DeliveryUserId)
                .OnDelete(DeleteBehavior.NoAction);


            // seeding initial data

            modelBuilder.Entity<UserModel>().HasData(
                new UserModel { Id=1, userName="zen2222", fullName="mohammed jalamnih", password="1111", phoneNumber="0598516128", deliveryLocation="jenin, abu-sufyan street", role=Role.Admin },
                new UserModel { Id = 2, userName ="muhaisen", fullName="Ahmed muhaisen",password="2222",phoneNumber="059851512",deliveryLocation="jenin, nablus street",role=Role.User },
                new UserModel { Id = 3, userName ="doe", fullName="john doe",password="3333",phoneNumber="056123423",deliveryLocation="Ramallah,jenin street",role=Role.Delivery },
                new UserModel { Id = 4, userName ="JD", fullName="jane doe",password="4444",phoneNumber="05678923412",deliveryLocation="nablus, jenin street",role=Role.Delivery },
                new UserModel { Id = 5, userName ="MJ", fullName="jickel mackson",password="5555",phoneNumber="05987234123",deliveryLocation="jerusalem, salah-al-deen street",role=Role.Admin }
                );
            modelBuilder.Entity<menuModel>().HasData(
                new menuModel
                {
                    Id = 1,
                    name = "Chicken Burger",
                    description = @"A chicken burger is a delicious and versatile sandwich made with a cooked chicken patty or fillet served inside a soft burger bun. Unlike a traditional beef burger, it offers a lighter but still satisfying flavor, and it can be prepared in many different ways.

There are two main types:

1. Crispy chicken burger – A breaded and deep-fried chicken fillet, giving a golden, crunchy exterior while staying juicy and tender inside. Often topped with lettuce, pickles, and mayo or spicy sauce.

2. Grilled chicken burger – A marinated, seared or grilled chicken breast that’s smokey, juicy, and lean. Often paired with fresh toppings like tomatoes, avocado, lettuce, and garlic yogurt or pesto sauce.

The bun is usually lightly toasted, and common toppings include cheese, red onions, pickles, lettuce, tomato, and sauces like ranch, buffalo, honey mustard, or barbecue.",
                    price = 31,
                    category = Categories.Burgers,
                    Imgpath = "uploads/Burger/Chicken Burger/chicken.jpg"
                },

                new menuModel
                {
                    Id=2,
                    name = "Smash Burger",
                    description = @"A Smash Burger is a style of burger known for its crispy, caramelized edges and juicy center. The name comes from the cooking technique: a ball of ground beef (usually 80/20 meat-to-fat ratio) is placed on a ripping hot griddle or pan, then smashed flat with a spatula — often until it’s very thin (about 3–6 mm).

This intense pressure creates maximum surface contact with the heat, triggering the Maillard reaction and forming a lacy, crunchy crust. Despite being thin, the patty stays tender inside.

Typically, two smashed patties are stacked with melted cheese in between, served on a soft, lightly toasted bun. Toppings are simple: pickles, onions, lettuce, ketchup, mustard, and mayo (or a special sauce).",
                    price = 32,
                    category = Categories.Burgers,
                    Imgpath = "uploads/Burger/Smash Burger/smash.jpg"
                },

                new menuModel
                {
                    Id = 3,
                    name = "Cheesecake",
                    description = @"Chocolate cake is a rich, moist, and decadent dessert made with cocoa or melted chocolate. It’s one of the most beloved cakes worldwide, known for its deep chocolate flavor and tender crumb.

Key characteristics:

· Texture: Soft, moist, and fluffy — often dense enough to feel satisfying but light enough to melt in your mouth
· Color: Deep brown, sometimes nearly black if made with dark cocoa
· Flavor: Sweet, rich, and intensely chocolatey — with notes of caramel, vanilla, and sometimes coffee (used to enhance the chocolate taste)

Common variations:

· With frosting: Layered or topped with chocolate buttercream, ganache, or cream cheese frosting
· Molten chocolate cake: A warm individual cake with a liquid, flowing center
· Flourless chocolate cake: Dense, fudgy, and gluten-free, almost like a truffle
· With fillings: Sometimes layered with chocolate mousse, raspberry jam, or caramel

Serving style:

· Served at room temperature or slightly warmed
· Often paired with vanilla ice cream, whipped cream, or fresh berries
· Dusted with powdered sugar or drizzled with chocolate sauce",
                    price = 15,
                    category = Categories.Desserts,
                    Imgpath = "uploads/Desserts/Cheesecake/cheese.jpg"
                },

                new menuModel
                {
                    Id = 4,
                    name = "Chocolate cake",
                    description = @"Chocolate cake is a rich, moist, and decadent dessert made with cocoa or melted chocolate. It’s one of the most beloved cakes worldwide, known for its deep chocolate flavor and tender crumb.

Key characteristics:

· Texture: Soft, moist, and fluffy — often dense enough to feel satisfying but light enough to melt in your mouth
· Color: Deep brown, sometimes nearly black if made with dark cocoa
· Flavor: Sweet, rich, and intensely chocolatey — with notes of caramel, vanilla, and sometimes coffee (used to enhance the chocolate taste)

Common variations:

· With frosting: Layered or topped with chocolate buttercream, ganache, or cream cheese frosting
· Molten chocolate cake: A warm individual cake with a liquid, flowing center
· Flourless chocolate cake: Dense, fudgy, and gluten-free, almost like a truffle
· With fillings: Sometimes layered with chocolate mousse, raspberry jam, or caramel

Serving style:

· Served at room temperature or slightly warmed
· Often paired with vanilla ice cream, whipped cream, or fresh berries
· Dusted with powdered sugar or drizzled with chocolate sauce",
                    price = 10,
                    category = Categories.Desserts,
                    Imgpath = "uploads/Desserts/Chocolate cake/ccake.jpg"
                },

                new menuModel
                {
                    Id = 5,
                    name = "Cola",
                    description = @"Cola is a carbonated, sweet, dark-colored soft drink known for its distinctive flavor — a balance of sweet vanilla, tangy citrus, warm cinnamon, and subtle caramel notes, often with a slight acidic bite. It’s one of the most recognized beverages in the world.

Key characteristics:

· Color: Deep caramel brown
· Carbonation: Bubbly and fizzy, adding a crisp, tingling sensation
· Sweetness: Usually high, often from sugar or high-fructose corn syrup (diet versions use artificial sweeteners)
· Caffeine: Contains a mild amount, adding a gentle energy lift
· Acidity: Slightly tangy, thanks to phosphoric acid, which balances the sweetness

Serving style:

· Served ice-cold over ice cubes
· Often paired with fast food, pizza, burgers, or barbecue
· Can be enjoyed on its own or used as a mixer in drinks like rum and cola (Cuba Libre)

Flavor profile in short:
Sweet, fizzy, slightly spicy (from cinnamon/vanilla notes), with a clean, sharp finish.",
                    price = 5,
                    category = Categories.Drinks,
                    Imgpath = "uploads/Drinks/Cola/cola.jpg"
                },

                new menuModel
                {
                    Id = 6,
                    name = "Iced Coffee",
                    description = @"Iced coffee is a refreshing, chilled coffee drink made by brewing hot coffee and then cooling it down — usually by pouring it over ice. It’s simple, crisp, and perfect for warm weather or when you want a cooler caffeine boost.

How it’s made:

· Freshly brewed hot coffee is chilled (either by refrigerating or pouring directly over ice)
· Often sweetened while still warm so the sugar dissolves easily
· Served in a glass full of ice cubes

Common additions:

· Milk, cream, or plant-based milk (oat, almond, soy)
· Sweeteners like simple syrup, sugar, vanilla syrup, or caramel
· Sometimes topped with whipped cream or a sprinkle of cinnamon

Flavor profile:

· Smooth, bold, and slightly less acidic than hot coffee
· The ice slowly dilutes it, creating a lighter, easy-to-drink taste

Difference from cold brew:
Unlike cold brew (which is steeped in cold water for 12–24 hours), iced coffee is brewed hot first — giving it a brighter, more traditional coffee flavor.",
                    price = 2,
                    category = Categories.Drinks,
                    Imgpath = "uploads/Drinks/Iced Coffee/iced.jpg"
                },

                new menuModel
                {
                    Id = 7,
                    name = "Seafood Pizza",
                    description = @"A seafood pizza is a delicious and flavorful twist on the classic Italian pizza, topped with a variety of ocean-fresh ingredients instead of traditional meats like pepperoni or sausage. It offers a lighter, brinier, and more delicate taste — perfect for seafood lovers.

Typical toppings include:

· Shrimp (prawns)
· Squid or calamari rings
· Mussels or clams (sometimes with shells on or off)
· Scallops or imitation crab (surimi)
· Occasionally octopus or anchovies

Base and cheese:

· Usually a thin or medium crust
· Tomato sauce or sometimes white garlic sauce (for a creamier, richer flavor)
· Mozzarella cheese, with possible parmesan or a sprinkle of herbs

Flavor profile:

· Savory, slightly sweet, and garlicky
· Often finished with fresh parsley, oregano, chili flakes, and a squeeze of lemon to brighten the seafood taste",
                    price = 35,
                    category = Categories.Pizza,
                    Imgpath = "uploads/Pizza/Seafood Pizza/seafood pizza.jpg"
                },

                new menuModel
                {
                    Id = 8,
                    name = "Supreme Pizza",
                    description = @"Typical toppings include:

· Meats: Pepperoni, Italian sausage (or ground beef), bacon, and sometimes ham
· Vegetables: Sliced bell peppers (green, red, or mixed), onions, black olives, mushrooms, and occasionally jalapeños for heat

Base and cheese:

· Classic tomato sauce
· Generous layer of mozzarella cheese
· Often finished with a sprinkle of oregano or parmesan

Flavor profile:

· Savory, slightly spicy, smoky, and earthy — with a satisfying mix of juicy, crispy, and chewy textures",
                    price = 35,
                    category = Categories.Pizza,
                    Imgpath = "uploads/Pizza/Supreme Pizza/supreme.jpg"
                }
            );



        }





    }
}
