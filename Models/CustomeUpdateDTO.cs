using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;

namespace Customer.Models;

public class CustomeUpdateDTO
{

	[BsonElement("firstName")]
	[JsonPropertyName("firstName")]
	public string FirstName { get; set; } = null!;

	[BsonElement("lastName")]
	[JsonPropertyName("lastName")]
	public string LastName { get; set; } = null!;

	[BsonElement("country")]
	[JsonPropertyName("country")]
	public string Country { get; set; } = null!;

	[BsonElement("state")]
	[JsonPropertyName("state")]
	public string State { get; set; } = null!;

	[BsonElement("city")]
	[JsonPropertyName("city")]
	public string City { get; set; } = null!;

}