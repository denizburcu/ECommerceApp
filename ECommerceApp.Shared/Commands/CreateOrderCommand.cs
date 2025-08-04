namespace ECommerceApp.Shared.Commands;

public class CreateOrderCommand
{
        /// <summary>
        /// Unique identifier for the order
        /// </summary>
        public string OrderId { get; init; }

        /// <summary>
        /// ProductId
        /// </summary>
        public string ProductId { get; init; }
        
        /// <summary>
        /// Amount to be blocked from the balance.
        /// </summary>
        public decimal Amount { get; init; }

        /// <summary>
        ///  CreatedAt
        /// </summary>
        public DateTime CreatedAt { get; init; }
    }
