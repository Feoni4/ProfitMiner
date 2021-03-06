﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProfitMiner.Core.Interfaces
{
    public interface IMiningPool
    {
        bool Supports24HEstimation { get; }

        Task<List<MiningPoolWalletState>> GetWalletState(string wallet);

        Task<List<MiningPoolAlogo>> GetAlgos();
    }
}