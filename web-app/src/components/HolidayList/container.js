import React, { Component } from 'react';
import { PropTypes as PT } from 'prop-types';
import { updateHoliday } from '../../services/holidayService';
import holidayStatus from '../../utilities/holidayStatus';
import Swal from 'sweetalert2';

export default Wrapped =>
  class extends Component {

    static propTypes = {
      holidays: PT.array,
      actions: PT.array,
    }

    static defaultProps = {
      holidays: [],
      actions: [],
    }

    constructor(props) {
      super(props);
      this.state = { holidays: props.holidays };
    }

    componentWillUpdate(nextProps) {
      if (nextProps.holidays != this.props.holidays) {
        this.setState({holidays: nextProps.holidays});
      }
    }

    approveHoliday = holiday => {
      if (holiday.holidayStatusId === holidayStatus.APPROVED) {
        Swal({title: 'Alert', text: 'Holiday already approved.'});
        return;
      }
      const approvedHoliday = { ...holiday };
      approvedHoliday.holidayStatusId = holidayStatus.APPROVED;
      const holidayIndex = this.state.holidays.indexOf(holiday);
      updateHoliday(approvedHoliday)
        .then(() => {
          const holidays = [...this.state.holidays];
          holidays[holidayIndex].holidayStatusId = holidayStatus.APPROVED;
          this.setState({ holidays });
        })
        .catch(error => {
          const holidays = [...this.state.holidays];
          holidays[holidayIndex].holidayStatusId = holiday.holidayStatusId;
          this.setState({ holidays });
          
          Swal({
            title: 'Could not approve holiday',
            text: error.message,
            type: 'error',
          });
        });
    };

    rejectHoliday = holiday => {
      if (holiday.holidayStatusId === holidayStatus.REJECTED) {
        Swal({title: 'Alert', text: 'Holiday already rejected.'});
        return;
      }
      const rejectedHoliday = { ...holiday };
      rejectedHoliday.holidayStatusId = holidayStatus.REJECTED;
      const holidayIndex = this.state.holidays.indexOf(holiday);
      updateHoliday(rejectedHoliday)
        .then(() => {
          const holidays = [...this.state.holidays];
          holidays[holidayIndex].holidayStatusId = holidayStatus.REJECTED;
          this.setState({ holidays });
        })
        .catch(error => {
          const holidays = [...this.state.holidays];
          holidays[holidayIndex].holidayStatusId = holiday.holidayStatusId;
          this.setState({ holidays });

          Swal({
            title: 'Could not reject holiday',
            text: error.message,
            type: 'error',
          });
        });
    };

    buildActions = () => {
      const actionStrings = this.props.actions;
      const actionFuncs = {};

      //NOTE: This is ALL actions available, add to this list if you add more
      const actions = {
        approve: this.approveHoliday,
        reject: this.rejectHoliday,
      };

      actionStrings.map(action => {
        actionFuncs[action] = actions[action];
      });

      return actionFuncs;
    }

    render() {
      return (
        <Wrapped
          {...this.props}
          {...this.state}
          approveHoliday={this.approveHoliday}
          rejectHoliday={this.rejectHoliday}
          actions={this.buildActions()}
        />
      );
    }
  };