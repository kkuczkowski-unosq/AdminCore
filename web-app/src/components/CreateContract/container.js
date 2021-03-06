import React, { Component } from 'react';
import { createContract } from '../../services/contractService';
import swal from 'sweetalert2';
import { Toast } from '../../utilities/Notifications';

export default Wrapped =>
  class extends Component {
    constructor(props) {
      super(props);
      this.state = {
        contractData: {},
        step: 0,
      };
    }

    updateFormState = formData => {
      this.setState({
        contractData: {
          ...this.state.contractData,
          ...formData,
        },
      });
    };

    nextStep = formData => {
      this.setState({
        step: this.state.step + 1,
      });

      if (formData) {
        this.updateFormState(formData);
      }
    };

    complete = () => {
      Toast({
        type: 'success',
        title: 'Contract created successfully! 👍',
      });
      this.setState({
        contractData: {},
        step: 0,
      });
    };

    submitContract = formData => {
      this.setState(
        {
          contractData: {
            ...this.state.contractData,
            ...formData,
          },
        },
        () => {
          const { contractData } = this.state;

          const contractRequest = {
            employeeId: contractData.selectedUser.value,
            teamId: contractData.selectedTeam.value,
            startDate: contractData.startDate.toISOString(),
            endDate: contractData.endDate.toISOString(),
          };

          if (contractData.isOpenEnded) {
            delete contractRequest.endDate;
          }

          createContract(contractRequest)
            .then(() => this.complete())
            .catch(error =>
              swal('Error Creating Contract', error.message, 'error')
            );
        }
      );
    };

    render() {
      return (
        <Wrapped
          {...this.props}
          step={this.state.step}
          nextStep={this.nextStep}
          submit={this.submitContract}
          contract={this.state.contractData}
        />
      );
    }
  };
