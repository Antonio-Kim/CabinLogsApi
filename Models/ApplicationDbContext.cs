using CabinLogsApi.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class ApplicationDbContext : IdentityDbContext<ApiUser>
{
	public ApplicationDbContext(DbContextOptions options) : base(options) { }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.Entity<Booking>()
			.HasKey(i => new { i.cabinId, i.guestId });
		modelBuilder.Entity<Booking>()
			.HasOne(c => c.Cabin)
			.WithMany(b => b.bookings)
			.HasForeignKey(k => k.cabinId)
			.IsRequired()
			.OnDelete(DeleteBehavior.Cascade);
		modelBuilder.Entity<Booking>()
			.HasOne(g => g.Guest)
			.WithMany(b => b.bookings)
			.HasForeignKey(k => k.guestId)
			.IsRequired()
			.OnDelete(DeleteBehavior.Cascade);

		// Cabin Data
		modelBuilder.Entity<Cabin>().HasData(
		new Cabin
		{
			id = 1,
			created_at = DateTime.UtcNow,
			name = "001",
			maxCapacity = 2,
			regularPrice = 250,
			discount = 0,
			description = "Discover the ultimate luxury getaway for couples in the cozy wooden cabin 001. Nestled in a picturesque forest, this stunning cabin offers a secluded and intimate retreat. Inside, enjoy modern high-quality wood interiors, a comfortable seating area, a fireplace and a fully-equipped kitchen. The plush king-size bed, dressed in fine linens guarantees a peaceful nights sleep. Relax in the spa-like shower and unwind on the private deck with hot tub.",
			image = "images/cabin-001.jpg"
		},
		new Cabin
		{
			id = 2,
			created_at = DateTime.UtcNow,
			name = "002",
			maxCapacity = 2,
			regularPrice = 350,
			discount = 25,
			description = "Escape to the serenity of nature and indulge in luxury in our cozy cabin 002. Perfect for couples, this cabin offers a secluded and intimate retreat in the heart of a picturesque forest. Inside, you will find warm and inviting interiors crafted from high-quality wood, a comfortable living area, a fireplace and a fully-equipped kitchen. The luxurious bedroom features a plush king-size bed and spa-like shower. Relax on the private deck with hot tub and take in the beauty of nature.",
			image = "images/cabin-002.jpg"
		},
		new Cabin
		{
			id = 3,
			created_at = DateTime.UtcNow,
			name = "003",
			maxCapacity = 4,
			regularPrice = 300,
			discount = 0,
			description = "Experience luxury family living in our medium-sized wooden cabin 003. Perfect for families of up to 4 people, this cabin offers a comfortable and inviting space with all modern amenities. Inside, you will find warm and inviting interiors crafted from high-quality wood, a comfortable living area, a fireplace, and a fully-equipped kitchen. The bedrooms feature plush beds and spa-like bathrooms. The cabin has a private deck with a hot tub and outdoor seating area, perfect for taking in the natural surroundings.",
			image = "images/cabin-003.jpg"
		},
		new Cabin
		{
			id = 4,
			created_at = DateTime.UtcNow,
			name = "004",
			maxCapacity = 4,
			regularPrice = 500,
			discount = 50,
			description = "Indulge in the ultimate luxury family vacation in this medium-sized cabin 004. Designed for families of up to 4, this cabin offers a sumptuous retreat for the discerning traveler. Inside, the cabin boasts of opulent interiors crafted from the finest quality wood, a comfortable living area, a fireplace, and a fully-equipped gourmet kitchen. The bedrooms are adorned with plush beds and spa-inspired en-suite bathrooms. Step outside to your private deck and soak in the natural surroundings while relaxing in your own hot tub.",
			image = "images/cabin-004.jpg"
		},
		new Cabin
		{
			id = 5,
			created_at = DateTime.UtcNow,
			name = "005",
			maxCapacity = 6,
			regularPrice = 350,
			discount = 0,
			description = "Enjoy a comfortable and cozy getaway with your group or family in our spacious cabin 005. Designed to accommodate up to 6 people, this cabin offers a secluded retreat in the heart of nature. Inside, the cabin features warm and inviting interiors crafted from quality wood, a living area with fireplace, and a fully-equipped kitchen. The bedrooms are comfortable and equipped with en-suite bathrooms. Step outside to your private deck and take in the natural surroundings while relaxing in your own hot tub.",
			image = "images/cabin-005.jpg"
		},
		new Cabin
		{
			id = 6,
			created_at = DateTime.UtcNow,
			name = "006",
			maxCapacity = 6,
			regularPrice = 800,
			discount = 100,
			description = "Experience the epitome of luxury with your group or family in our spacious wooden cabin 006. Designed to comfortably accommodate up to 6 people, this cabin offers a lavish retreat in the heart of nature. Inside, the cabin features opulent interiors crafted from premium wood, a grand living area with fireplace, and a fully-equipped gourmet kitchen. The bedrooms are adorned with plush beds and spa-like en-suite bathrooms. Step outside to your private deck and soak in the natural surroundings while relaxing in your own hot tub.",
			image = "images/cabin-006.jpg"
		},
		new Cabin
		{
			id = 7,
			created_at = DateTime.UtcNow,
			name = "007",
			maxCapacity = 8,
			regularPrice = 600,
			discount = 100,
			description = "Accommodate your large group or multiple families in the spacious and grand wooden cabin 007. Designed to comfortably fit up to 8 people, this cabin offers a secluded retreat in the heart of beautiful forests and mountains. Inside, the cabin features warm and inviting interiors crafted from quality wood, multiple living areas with fireplace, and a fully-equipped kitchen. The bedrooms are comfortable and equipped with en-suite bathrooms. The cabin has a private deck with a hot tub and outdoor seating area, perfect for taking in the natural surroundings.",
			image = "images/cabin-007.jpg"
		},
		new Cabin
		{
			id = 8,
			created_at = DateTime.UtcNow,
			name = "008",
			maxCapacity = 10,
			regularPrice = 1400,
			discount = 0,
			description = "Experience the epitome of luxury and grandeur with your large group or multiple families in our grand cabin 008. This cabin offers a lavish retreat that caters to all your needs and desires. The cabin features an opulent design and boasts of high-end finishes, intricate details and the finest quality wood throughout. Inside, the cabin features multiple grand living areas with fireplaces, a formal dining area, and a gourmet kitchen that is a chef's dream. The bedrooms are designed for ultimate comfort and luxury, with plush beds and en-suite spa-inspired bathrooms. Step outside and immerse yourself in the beauty of nature from your private deck, featuring a luxurious hot tub and ample seating areas for ultimate relaxation and enjoyment.",
			image = "images/cabin-008.jpg"
		}
	);

		// Guest Data
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 1,
			created_at = DateTime.UtcNow,
			fullName = "Jonas Schmedtmann",
			email = "hello@jonas.io",
			nationality = "Portugal",
			nationalId = "3525436345",
			countryFlag = "https://flagcdn.com/pt.svg"
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 2,
			created_at = DateTime.UtcNow,
			fullName = "Jonathan Smith",
			email = "johnsmith@test.eu",
			nationality = "Great Britain",
			nationalId = "4534593454",
			countryFlag = "https://flagcdn.com/gb.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 3,
			created_at = DateTime.UtcNow,
			fullName = "Jonatan Johansson",
			email = "jonatan@example.com",
			nationality = "Finland",
			nationalId = "9374074454",
			countryFlag = "https://flagcdn.com/fi.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 4,
			created_at = DateTime.UtcNow,
			fullName = "Jonas Mueller",
			email = "jonas@example.eu",
			nationality = "Germany",
			nationalId = "1233212288",
			countryFlag = "https://flagcdn.com/de.svg"
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 5,
			created_at = DateTime.UtcNow,
			fullName = "Jonas Anderson",
			email = "anderson@example.com",
			nationality = "Bolivia (Plurinational State of)",
			nationalId = "0988520146",
			countryFlag = "https://flagcdn.com/bo.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 6,
			created_at = DateTime.UtcNow,
			fullName = "Jonathan Williams",
			email = "jowi@gmail.com",
			nationality = "United States of America",
			nationalId = "633678543",
			countryFlag = "https://flagcdn.com/us.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 7,
			created_at = DateTime.UtcNow,
			fullName = "Emma Watson",
			email = "emma@gmail.com",
			nationality = "United Kingdom",
			nationalId = "1234578901",
			countryFlag = "https://flagcdn.com/gb.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 8,
			created_at = DateTime.UtcNow,
			fullName = "Mohammed Ali",
			email = "mohammedali@yahoo.com",
			nationality = "Egypt",
			nationalId = "987543210",
			countryFlag = "https://flagcdn.com/eg.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 9,
			created_at = DateTime.UtcNow,
			fullName = "Maria Rodriguez",
			email = "maria@gmail.com",
			nationality = "Spain",
			nationalId = "1098765321",
			countryFlag = "https://flagcdn.com/es.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 10,
			created_at = DateTime.UtcNow,
			fullName = "Li Mei",
			email = "li.mei@hotmail.com",
			nationality = "China",
			nationalId = "102934756",
			countryFlag = "https://flagcdn.com/cn.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 11,
			created_at = DateTime.UtcNow,
			fullName = "Khadija Ahmed",
			email = "khadija@gmail.com",
			nationality = "Sudan",
			nationalId = "1023457890",
			countryFlag = "https://flagcdn.com/sd.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 12,
			created_at = DateTime.UtcNow,
			fullName = "Gabriel Silva",
			email = "gabriel@gmail.com",
			nationality = "Brazil",
			nationalId = "109283465",
			countryFlag = "https://flagcdn.com/br.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 13,
			created_at = DateTime.UtcNow,
			fullName = "Maria Gomez",
			email = "maria@example.com",
			nationality = "Mexico",
			nationalId = "108765421",
			countryFlag = "https://flagcdn.com/mx.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 14,
			created_at = DateTime.UtcNow,
			fullName = "Ahmed Hassan",
			email = "ahmed@gmail.com",
			nationality = "Egypt",
			nationalId = "1077777777",
			countryFlag = "https://flagcdn.com/eg.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 15,
			created_at = DateTime.UtcNow,
			fullName = "John Doe",
			email = "johndoe@gmail.com",
			nationality = "United States",
			nationalId = "3245908744",
			countryFlag = "https://flagcdn.com/us.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 16,
			created_at = DateTime.UtcNow,
			fullName = "Fatima Ahmed",
			email = "fatima@example.com",
			nationality = "Pakistan",
			nationalId = "1089999363",
			countryFlag = "https://flagcdn.com/pk.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 17,
			created_at = DateTime.UtcNow,
			fullName = "David Smith",
			email = "david@gmail.com",
			nationality = "Australia",
			nationalId = "44450960283",
			countryFlag = "https://flagcdn.com/au.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 18,
			created_at = DateTime.UtcNow,
			fullName = "Marie Dupont",
			email = "marie@gmail.com",
			nationality = "France",
			nationalId = "06934233728",
			countryFlag = "https://flagcdn.com/fr.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 19,
			created_at = DateTime.UtcNow,
			fullName = "Ramesh Patel",
			email = "ramesh@gmail.com",
			nationality = "India",
			nationalId = "9875412303",
			countryFlag = "https://flagcdn.com/in.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 20,
			created_at = DateTime.UtcNow,
			fullName = "Fatimah Al-Sayed",
			email = "fatimah@gmail.com",
			nationality = "Kuwait",
			nationalId = "0123456789",
			countryFlag = "https://flagcdn.com/kw.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 21,
			created_at = DateTime.UtcNow,
			fullName = "Nina Williams",
			email = "nina@hotmail.com",
			nationality = "South Africa",
			nationalId = "2345678901",
			countryFlag = "https://flagcdn.com/za.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 22,
			created_at = DateTime.UtcNow,
			fullName = "Taro Tanaka",
			email = "taro@gmail.com",
			nationality = "Japan",
			nationalId = "3456789012",
			countryFlag = "https://flagcdn.com/jp.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 23,
			created_at = DateTime.UtcNow,
			fullName = "Abdul Rahman",
			email = "abdul@gmail.com",
			nationality = "Saudi Arabia",
			nationalId = "4567890123",
			countryFlag = "https://flagcdn.com/sa.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 24,
			created_at = DateTime.UtcNow,
			fullName = "Julie Nguyen",
			email = "julie@gmail.com",
			nationality = "Vietnam",
			nationalId = "5678901234",
			countryFlag = "https://flagcdn.com/vn.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 25,
			created_at = DateTime.UtcNow,
			fullName = "Sara Lee",
			email = "sara@gmail.com",
			nationality = "South Korea",
			nationalId = "6789012345",
			countryFlag = "https://flagcdn.com/kr.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 26,
			created_at = DateTime.UtcNow,
			fullName = "Carlos Gomez",
			email = "carlos@yahoo.com",
			nationality = "Colombia",
			nationalId = "7890123456",
			countryFlag = "https://flagcdn.com/co.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 27,
			created_at = DateTime.UtcNow,
			fullName = "Emma Brown",
			email = "emma@gmail.com",
			nationality = "Canada",
			nationalId = "8901234567",
			countryFlag = "https://flagcdn.com/ca.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 28,
			created_at = DateTime.UtcNow,
			fullName = "Juan Hernandez",
			email = "juan@yahoo.com",
			nationality = "Argentina",
			nationalId = "4343433333",
			countryFlag = "https://flagcdn.com/ar.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 29,
			created_at = DateTime.UtcNow,
			fullName = "Ibrahim Ahmed",
			email = "ibrahim@yahoo.com",
			nationality = "Nigeria",
			nationalId = "2345678009",
			countryFlag = "https://flagcdn.com/ng.svg",
		});
		modelBuilder.Entity<Guest>().HasData(new Guest
		{
			id = 30,
			created_at = DateTime.UtcNow,
			fullName = "Mei Chen",
			email = "mei@gmail.com",
			nationality = "Taiwan",
			nationalId = "3456117890",
			countryFlag = "https://flagcdn.com/tw.svg",
		});


		// Setting Data
		modelBuilder.Entity<Setting>().HasData(new Setting
		{
			id = 1,
			created_at = DateTime.UtcNow,
			minBookingLength = 3,
			maxBookingLength = 90,
			breakfastPrice = 15,
		});

		// Booking Data
		modelBuilder.Entity<Booking>().HasData(
		new Booking
		{
			id = 1,
			created_at = DateTime.UtcNow.AddDays(-20),
			startDate = DateTime.UtcNow,
			endDate = DateTime.UtcNow.AddDays(7),
			numberOfNights = 7,
			numGuests = 1,
			cabinPrice = 1750,
			extrasPrice = 105,
			totalPrice = 1855,
			status = "checked-in",
			hasBreakfast = true,
			isPaid = false,
			observations = "I have a gluten allergy and would like to request a gluten-free breakfast.",
			cabinId = 1,
			guestId = 2
		},
		new Booking
		{
			id = 2,
			created_at = DateTime.UtcNow.AddDays(-33),
			startDate = DateTime.UtcNow.AddDays(-23),
			endDate = DateTime.UtcNow.AddDays(-13),
			numberOfNights = 10,
			numGuests = 2,
			cabinPrice = 2500,
			extrasPrice = 300,
			totalPrice = 2800,
			status = "checked-out",
			hasBreakfast = true,
			isPaid = true,
			observations = "",
			cabinId = 1,
			guestId = 3
		},
		new Booking
		{
			id = 3,
			created_at = DateTime.UtcNow.AddDays(-27),
			startDate = DateTime.UtcNow.AddDays(12),
			endDate = DateTime.UtcNow.AddDays(18),
			numberOfNights = 6,
			numGuests = 2,
			cabinPrice = 1500,
			extrasPrice = 180,
			totalPrice = 1680,
			status = "confirmed",
			hasBreakfast = false,
			isPaid = false,
			observations = "",
			cabinId = 1,
			guestId = 4
		},
		new Booking
		{
			id = 4,
			created_at = DateTime.UtcNow.AddDays(-45),
			startDate = DateTime.UtcNow.AddDays(-45),
			endDate = DateTime.UtcNow.AddDays(-29),
			numberOfNights = 16,
			numGuests = 2,
			cabinPrice = 5200,
			extrasPrice = 480,
			totalPrice = 5680,
			status = "checked-out",
			hasBreakfast = false,
			isPaid = true,
			observations = "",
			cabinId = 2,
			guestId = 5
		},
		new Booking
		{
			id = 5,
			created_at = DateTime.UtcNow.AddDays(-2),
			startDate = DateTime.UtcNow.AddDays(15),
			endDate = DateTime.UtcNow.AddDays(18),
			numberOfNights = 3,
			numGuests = 2,
			cabinPrice = 975,
			extrasPrice = 90,
			totalPrice = 1065,
			status = "unconfirmed",
			hasBreakfast = true,
			isPaid = true,
			observations = "",
			cabinId = 2,
			guestId = 6
		},
		new Booking
		{
			id = 6,
			created_at = DateTime.UtcNow.AddDays(-5),
			startDate = DateTime.UtcNow.AddDays(33),
			endDate = DateTime.UtcNow.AddDays(48),
			numberOfNights = 15,
			numGuests = 2,
			cabinPrice = 4875,
			extrasPrice = 450,
			totalPrice = 5325,
			status = "confirmed",
			hasBreakfast = true,
			isPaid = false,
			observations = "",
			cabinId = 2,
			guestId = 7
		},
		new Booking
		{
			id = 7,
			created_at = DateTime.UtcNow.AddDays(-65),
			startDate = DateTime.UtcNow.AddDays(-25),
			endDate = DateTime.UtcNow.AddDays(-20),
			numberOfNights = 5,
			numGuests = 4,
			cabinPrice = 1500,
			extrasPrice = 300,
			totalPrice = 1800,
			status = "checked-out",
			hasBreakfast = true,
			isPaid = true,
			observations = "",
			cabinId = 3,
			guestId = 8
		},
		new Booking
		{
			id = 8,
			created_at = DateTime.UtcNow.AddDays(-2),
			startDate = DateTime.UtcNow.AddDays(-2),
			endDate = DateTime.UtcNow,
			numberOfNights = 2,
			numGuests = 3,
			cabinPrice = 600,
			extrasPrice = 90,
			totalPrice = 690,
			status = "checked-in",
			hasBreakfast = false,
			isPaid = true,
			observations = "We will be bringing our small dog with us",
			cabinId = 3,
			guestId = 9
		},
		new Booking
		{
			id = 9,
			created_at = DateTime.UtcNow.AddDays(-14),
			startDate = DateTime.UtcNow.AddDays(-14),
			endDate = DateTime.UtcNow.AddDays(-11),
			numberOfNights = 3,
			numGuests = 4,
			cabinPrice = 900,
			extrasPrice = 180,
			totalPrice = 1080,
			status = "checked-out",
			hasBreakfast = true,
			isPaid = true,
			observations = "",
			cabinId = 3,
			guestId = 10
		},
		new Booking
		{
			id = 10,
			created_at = DateTime.UtcNow.AddDays(-30),
			startDate = DateTime.UtcNow.AddDays(-4),
			endDate = DateTime.UtcNow.AddDays(8),
			numberOfNights = 12,
			numGuests = 4,
			cabinPrice = 5400,
			extrasPrice = 720,
			totalPrice = 6120,
			status = "checked-in",
			hasBreakfast = true,
			isPaid = true,
			observations = "",
			cabinId = 4,
			guestId = 11
		},
		new Booking
		{
			id = 11,
			created_at = DateTime.UtcNow.AddDays(-1),
			startDate = DateTime.UtcNow.AddDays(12),
			endDate = DateTime.UtcNow.AddDays(17),
			numberOfNights = 5,
			numGuests = 4,
			cabinPrice = 2250,
			extrasPrice = 300,
			totalPrice = 2550,
			status = "unconfirmed",
			hasBreakfast = true,
			isPaid = true,
			observations = "",
			cabinId = 4,
			guestId = 12
		},
		new Booking
		{
			id = 12,
			created_at = DateTime.UtcNow.AddDays(-7),
			startDate = DateTime.UtcNow.AddDays(-1),
			endDate = DateTime.UtcNow.AddDays(3),
			numberOfNights = 4,
			numGuests = 4,
			cabinPrice = 1800,
			extrasPrice = 240,
			totalPrice = 2040,
			status = "checked-in",
			hasBreakfast = false,
			isPaid = false,
			observations = "",
			cabinId = 4,
			guestId = 13
		},
		new Booking
		{
			id = 13,
			created_at = DateTime.UtcNow.AddDays(-14),
			startDate = DateTime.UtcNow.AddDays(-12),
			endDate = DateTime.UtcNow.AddDays(-8),
			numberOfNights = 4,
			numGuests = 6,
			cabinPrice = 1400,
			extrasPrice = 360,
			totalPrice = 1760,
			status = "checked-out",
			hasBreakfast = true,
			isPaid = true,
			observations = "",
			cabinId = 5,
			guestId = 14
		},
		new Booking
		{
			id = 14,
			created_at = DateTime.UtcNow.AddDays(-6),
			startDate = DateTime.UtcNow.AddDays(-2),
			endDate = DateTime.UtcNow.AddDays(2),
			numberOfNights = 4,
			numGuests = 6,
			cabinPrice = 1400,
			extrasPrice = 360,
			totalPrice = 1760,
			status = "checked-in",
			hasBreakfast = false,
			isPaid = true,
			observations = "",
			cabinId = 5,
			guestId = 15
		},
		new Booking
		{
			id = 15,
			created_at = DateTime.UtcNow.AddDays(-30),
			startDate = DateTime.UtcNow.AddDays(-30),
			endDate = DateTime.UtcNow.AddDays(-23),
			numberOfNights = 7,
			numGuests = 6,
			cabinPrice = 2450,
			extrasPrice = 630,
			totalPrice = 3080,
			status = "checked-out",
			hasBreakfast = true,
			isPaid = true,
			observations = "",
			cabinId = 5,
			guestId = 16
		},
		new Booking
		{
			id = 16,
			created_at = DateTime.UtcNow.AddDays(-40),
			startDate = DateTime.UtcNow.AddDays(-20),
			endDate = DateTime.UtcNow.AddDays(-13),
			numberOfNights = 7,
			numGuests = 6,
			cabinPrice = 4900,
			extrasPrice = 630,
			totalPrice = 5530,
			status = "checked-out",
			hasBreakfast = true,
			isPaid = true,
			observations = "",
			cabinId = 6,
			guestId = 17
		},
		new Booking
		{
			id = 17,
			created_at = DateTime.UtcNow.AddDays(-10),
			startDate = DateTime.UtcNow.AddDays(2),
			endDate = DateTime.UtcNow.AddDays(10),
			numberOfNights = 8,
			numGuests = 3,
			cabinPrice = 5600,
			extrasPrice = 360,
			totalPrice = 5960,
			status = "confirmed",
			hasBreakfast = true,
			isPaid = false,
			observations = "",
			cabinId = 6,
			guestId = 18
		},
		new Booking
		{
			id = 18,
			created_at = DateTime.UtcNow.AddDays(-2),
			startDate = DateTime.UtcNow.AddDays(-1),
			endDate = DateTime.UtcNow.AddDays(3),
			numberOfNights = 4,
			numGuests = 6,
			cabinPrice = 2800,
			extrasPrice = 360,
			totalPrice = 3160,
			status = "checked-in",
			hasBreakfast = false,
			isPaid = false,
			observations = "",
			cabinId = 6,
			guestId = 19
		},
		new Booking
		{
			id = 19,
			created_at = DateTime.UtcNow.AddDays(-10),
			startDate = DateTime.UtcNow.AddDays(-8),
			endDate = DateTime.UtcNow.AddDays(-4),
			numberOfNights = 4,
			numGuests = 8,
			cabinPrice = 2000,
			extrasPrice = 480,
			totalPrice = 2480,
			status = "checked-out",
			hasBreakfast = true,
			isPaid = true,
			observations = "",
			cabinId = 7,
			guestId = 20
		},
		new Booking
		{
			id = 20,
			created_at = DateTime.UtcNow.AddDays(-5),
			startDate = DateTime.UtcNow.AddDays(3),
			endDate = DateTime.UtcNow.AddDays(10),
			numberOfNights = 7,
			numGuests = 8,
			cabinPrice = 3500,
			extrasPrice = 840,
			totalPrice = 4340,
			status = "confirmed",
			isPaid = true,
			observations = "",
			cabinId = 7,
			guestId = 21
		},
		new Booking
		{
			id = 21,
			created_at = DateTime.UtcNow.AddDays(-8),
			startDate = DateTime.UtcNow.AddDays(-4),
			endDate = DateTime.UtcNow.AddDays(0),
			numberOfNights = 4,
			numGuests = 8,
			cabinPrice = 2000,
			extrasPrice = 480,
			totalPrice = 2480,
			status = "checked-in",
			isPaid = false,
			observations = "",
			cabinId = 7,
			guestId = 22
		},
		new Booking
		{
			id = 22,
			created_at = DateTime.UtcNow.AddDays(-30),
			startDate = DateTime.UtcNow.AddDays(-20),
			endDate = DateTime.UtcNow.AddDays(-15),
			numberOfNights = 5,
			numGuests = 10,
			cabinPrice = 7000,
			extrasPrice = 750,
			totalPrice = 7750,
			status = "checked-out",
			hasBreakfast = false,
			isPaid = true,
			observations = "",
			cabinId = 8,
			guestId = 23
		},
		new Booking
		{
			id = 23,
			created_at = DateTime.UtcNow.AddDays(-10),
			startDate = DateTime.UtcNow.AddDays(-3),
			endDate = DateTime.UtcNow.AddDays(4),
			numberOfNights = 7,
			numGuests = 10,
			cabinPrice = 9800,
			extrasPrice = 1050,
			totalPrice = 10850,
			status = "checked-in",
			hasBreakfast = true,
			isPaid = true,
			observations = "",
			cabinId = 8,
			guestId = 24
		},
		new Booking
		{
			id = 24,
			created_at = DateTime.UtcNow.AddDays(-5),
			startDate = DateTime.UtcNow.AddDays(1),
			endDate = DateTime.UtcNow.AddDays(4),
			numberOfNights = 3,
			numGuests = 10,
			cabinPrice = 4200,
			extrasPrice = 450,
			totalPrice = 4650,
			status = "confirmed",
			hasBreakfast = true,
			isPaid = false,
			observations = "",
			cabinId = 8,
			guestId = 25
		});
	}

	public DbSet<Cabin> Cabins => Set<Cabin>();
	public DbSet<Guest> Guests => Set<Guest>();
	public DbSet<Setting> Settings => Set<Setting>();
	public DbSet<Booking> Bookings => Set<Booking>();
}