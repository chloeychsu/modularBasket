using Shared.Exceptions;

namespace Basket;

public class BasketNotFoundException(string userName):NotFoundException("ShoppingCart",userName);