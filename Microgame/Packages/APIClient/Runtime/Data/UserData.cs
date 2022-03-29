using System;

[Serializable]
public class UserData
{
	public int id;
	public string name;
	public string username;
	public string email;
	public UserAddress address;
	public string phone;
	public string website;
	public UserCompany company;

	public UserData(int id, string name, string username, string email, UserAddress address, string phone, string website, UserCompany company)
	{
		this.id = id;
		this.name = name;
		this.username = username;
		this.email = email;
		this.address = address;
		this.phone = phone;
		this.website = website;
		this.company = company;
	}
}

[Serializable]
public class UserAddress
{
	public string street;
	public string suite;
	public string city;
	public string zipcode;
	public UserGeoLocation geo;

	public UserAddress(string street, string suite, string city, string zipcode, UserGeoLocation geo)
	{
		this.street = street;
		this.suite = suite;
		this.city = city;
		this.zipcode = zipcode;
		this.geo = geo;
	}
}

[Serializable]
public class UserCompany
{
	public string name;
	public string catchPhrase;
	public string bs;

	public UserCompany(string name, string catchPhrase, string bs)
	{
		this.name = name;
		this.catchPhrase = catchPhrase;
		this.bs = bs;
	}
}

[Serializable]
public class UserGeoLocation
{
	public string lat;
	public string lng;


	public UserGeoLocation(string title, string body)
	{
		this.lat = title;
		this.lng = body;
	}
}