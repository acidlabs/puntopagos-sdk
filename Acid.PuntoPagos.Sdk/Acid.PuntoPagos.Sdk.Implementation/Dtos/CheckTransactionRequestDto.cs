namespace Acid.PuntoPagos.Sdk.Dtos
{
    public class CheckTransactionRequestDto
    {
        /// <summary>
        /// Unique identifier of the client's transaction
        /// </summary>
        public string Id { get; private set; }
        /// <summary>
        /// Total value of the transaction
        /// </summary>
        public CurrencyDto Currency { get; private set; }
        /// <summary>
        /// Unique identifier of the transaction Payment Point
        /// </summary>
        public string Token { get; private set; }

        /// <summary>
        /// Create new object for check the result of transacction process
        /// </summary>
        /// <param name="mount">Total value of the transaction</param>
        /// <param name="transactionId">Unique identifier of the client's transaction</param>
        /// <param name="token">Unique identifier of the transaction Payment Point</param>
        public CheckTransactionRequestDto(decimal mount, string transactionId, string token)
        {
            Id = transactionId;
            Currency = new CurrencyDto(mount);
            Token = token;
        } 
    }
}