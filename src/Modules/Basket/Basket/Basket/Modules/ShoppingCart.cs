﻿namespace Basket.Basket.Modules;

public class ShoppingCart : Aggregate<Guid>
{
     public string UserName { get; set; } = default!;
     private readonly List<ShoppingCartItem> _items = new();
     public IReadOnlyList<ShoppingCartItem> Items => _items.AsReadOnly();
     public decimal TotalPrice => Items.Sum(x => x.Price * x.Quantity);

     public static ShoppingCart Create(Guid id, string userName)
     {
          ArgumentException.ThrowIfNullOrEmpty(userName);
          var shoppingCart = new ShoppingCart
          {
               Id = id,
               UserName = userName
          };
          return shoppingCart;
     }
     public void AddItem(Guid productId, int quantity, string color, decimal price, string productName)
     {
          ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantity);
          ArgumentOutOfRangeException.ThrowIfNegativeOrZero(price);

          var existingItem = Items.FirstOrDefault(x => x.ProductId == productId);
          if (existingItem != null)
          {
               existingItem.Quantity += quantity;
          }
          else
          {
               _items.Add(new ShoppingCartItem(Id, productId, quantity, color, price, productName));
          }
     }
     public void RemoveItem(Guid productId)
     {
          var existingItem = Items.FirstOrDefault(x => x.ProductId == productId);
          if (existingItem != null)
          {
               _items.Remove(existingItem);
          }
     }
}