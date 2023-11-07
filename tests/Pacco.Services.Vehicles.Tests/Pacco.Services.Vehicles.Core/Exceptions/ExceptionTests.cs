using Pacco.Services.Vehicles.Core.Exceptions;

namespace Pacco.Services.Vehicles.Tests.Pacco.Services.Vehicles.Core.Exceptions;

public class ExceptionTests
{
	private static void Throws_InvalidVehicleCapacity(int value)
	{
		throw new InvalidVehicleCapacity(value);
	}

	private static void Throws_InvalidVehicleDescriptionException(string value)
	{
		throw new InvalidVehicleDescriptionException(value);
	}

	private static void Throws_InvalidVehiclePricePerServiceException(decimal value)
	{
		throw new InvalidVehiclePricePerServiceException(value);
	}

	[Fact]
	public void InvalidVehicleCapacity_Returns_Message_And_Code()
	{
		var capacity = 2;

		// ACT
		var exception = Assert.Throws<InvalidVehicleCapacity>(() => Throws_InvalidVehicleCapacity(capacity));

		//ASSERTS
		exception.ShouldNotBeNull();
		exception.ShouldBeOfType<InvalidVehicleCapacity>();
		exception.Message.ShouldBe($"Vehicle capacity is invalid: {capacity}.");
		exception.Code.ShouldBe("invalid_vehicle_capacity");
	}

	[Fact]
	public void InvalidVehicleDescriptionException_Returns_Message_And_Code()
	{
		// ARRANGE
		var description = "Description";

		// ACT
		var exception = Assert.Throws<InvalidVehicleDescriptionException>(() => Throws_InvalidVehicleDescriptionException(description));

		//ASSERTS
		exception.ShouldNotBeNull();
		exception.ShouldBeOfType<InvalidVehicleDescriptionException>();
		exception.Message.ShouldBe($"Vehicle description is invalid: {description}.");
		exception.Code.ShouldBe("invalid_vehicle_description");
	}

	[Fact]
	public void InvalidVehiclePricePerServiceException_Returns_Message_And_Code()
	{
		// ARRANGE
		var price = 20.0m;

		// ACT
		var exception = Assert.Throws<InvalidVehiclePricePerServiceException>(() => Throws_InvalidVehiclePricePerServiceException(price));

		//ASSERTS
		exception.ShouldNotBeNull();
		exception.ShouldBeOfType<InvalidVehiclePricePerServiceException>();
		exception.Message.ShouldBe($"Vehicle price per service is invalid: {price}.");
		exception.Code.ShouldBe("invalid_vehicle_price_per_service");
	}


}
