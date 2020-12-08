﻿using System;
using System.Threading.Tasks;
using Website.Database.Entities;

namespace Website.Services
{
    public class CommentCreationService
    {
        private readonly PaymentRepository _paymentRepository;
        private readonly CommentRepository _commentRepository;

        public CommentCreationService(PaymentRepository paymentRepository, CommentRepository commentRepository)
        {
            this._paymentRepository = paymentRepository;
            this._commentRepository = commentRepository;
        }

        public async Task<Comment> AddComment(Guid paymentId, string nickname, string message)
        {
            var payment = await _paymentRepository.GetById(paymentId);
            if (payment == null)
            {
                throw new InvalidOperationException("Attempted to post a comment without an existing payment");
            }

            var existingComment = await _commentRepository.GetByPaymentId(paymentId);
            if (existingComment != null)
            {
                // TODO: make this user-friendly
                throw new InvalidOperationException("This payment already has a comment");
            }

            return await _commentRepository.Create(paymentId, nickname, message);
        }
    }
}
