using AutoMapper;
using PruebaTecnicaBank.Core.DTOs;
using PruebaTecnicaBank.Core.Entities;
using PruebaTecnicaBank.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PruebaTecnicaBank.Core.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepo;
        private readonly ITransactionRepository _transactionRepo;
        private readonly IClientRepository _clientRepo;
        private readonly IMapper _mapper;

        public AccountService(
            IAccountRepository accountRepo,
            ITransactionRepository transactionRepo,
            IClientRepository clientRepo,
            IMapper mapper)
        {
            _accountRepo = accountRepo;
            _transactionRepo = transactionRepo;
            _clientRepo = clientRepo;
            _mapper = mapper;
        }

        public async Task<AccountResponseDto> CreateAccountAsync(AccountCreateDto accountDto)
        {
            var client = await _clientRepo.GetByIdAsync(accountDto.ClientId);
            if (client == null) throw new Exception("Cliente no encontrado");

            var account = _mapper.Map<Account>(accountDto);
            account.Id = Guid.NewGuid();
            account.AccountNumber = Guid.NewGuid().ToString().Substring(0, 10);

            var createdAccount = await _accountRepo.AddAsync(account);
            return _mapper.Map<AccountResponseDto>(createdAccount);
        }

        public async Task<decimal> GetBalanceAsync(string accountNumber)
        {
            var account = await _accountRepo.GetByNumberAsync(accountNumber);
            if (account == null) throw new Exception("Cuenta no encontrada");
            return account.Balance;
        }

        public async Task<TransactionResponseDto> DepositAsync(string accountNumber, TransactionCreateDto transactionDto)
        {
            var account = await _accountRepo.GetByNumberAsync(accountNumber);
            if (account == null) throw new Exception("Cuenta no encontrada");

            account.Balance += transactionDto.Amount;

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                Type = TransactionType.Deposit,
                Amount = transactionDto.Amount,
                BalanceAfter = account.Balance,
                Timestamp = DateTime.UtcNow
            };

            await _transactionRepo.AddAsync(transaction);
            await _accountRepo.UpdateAsync(account);

            return _mapper.Map<TransactionResponseDto>(transaction);
        }

        public async Task<TransactionResponseDto> WithdrawAsync(string accountNumber, TransactionCreateDto transactionDto)
        {
            var account = await _accountRepo.GetByNumberAsync(accountNumber);
            if (account == null) throw new Exception("Cuenta no encontrada");
            if (account.Balance < transactionDto.Amount) throw new Exception("Fondos insuficientes");

            account.Balance -= transactionDto.Amount;

            var transaction = new Transaction
            {
                Id = Guid.NewGuid(),
                AccountId = account.Id,
                Type = TransactionType.Withdrawal,
                Amount = transactionDto.Amount,
                BalanceAfter = account.Balance,
                Timestamp = DateTime.UtcNow
            };

            await _transactionRepo.AddAsync(transaction);
            await _accountRepo.UpdateAsync(account);

            return _mapper.Map<TransactionResponseDto>(transaction);
        }

        public async Task<IEnumerable<TransactionResponseDto>> GetTransactionsAsync(string accountNumber)
        {
            var account = await _accountRepo.GetByNumberAsync(accountNumber);
            if (account == null) throw new Exception("Cuenta no encontrada");

            var transactions = await _transactionRepo.GetByAccountAsync(account.Id);
            return _mapper.Map<IEnumerable<TransactionResponseDto>>(transactions);
        }
    }
}
