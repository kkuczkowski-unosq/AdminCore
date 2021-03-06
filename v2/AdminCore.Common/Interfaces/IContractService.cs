﻿using AdminCore.DTOs;
using System.Collections.Generic;

namespace AdminCore.Common.Interfaces
{
  public interface IContractService
  {
    ContractDto GetContractById(int contractId);

    IList<ContractDto> GetContractByEmployeeId(int employeeId);

    IList<ContractDto> GetContractByTeamId(int teamId);

    IList<ContractDto> GetContractByEmployeeIdAndTeamId(int employeeId, int teamId);

    void SaveContract(ContractDto contractToBeSaved);

    void DeleteContract(int contractId);
  }
}