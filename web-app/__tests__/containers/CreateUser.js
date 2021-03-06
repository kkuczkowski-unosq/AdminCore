import React from 'react';
import { shallow } from 'enzyme';
import moment from 'moment';
import { CreateUser } from '../../src/components';

jest.mock('../../src/services/userService');
import * as userService from '../../src/services/userService';

xdescribe('CreateUser', () => {
  it('renders correctly', () => {
    const wrapper = shallow(<CreateUser />);

    expect(wrapper.exists());
    checkIfFormEqualsInitialState(wrapper);
    expect(wrapper.prop('submitForm')).toEqual(
      wrapper.instance().handleFormSubmit,
    );
    expect(wrapper.prop('formChanged')).toEqual(
      wrapper.instance().handleFormChange,
    );
    expect(wrapper.prop('startDateChanged')).toEqual(
      wrapper.instance().handleStartDateChange,
    );
    expect(wrapper.prop('success')).toEqual(null);
    expect(wrapper.prop('error')).toEqual(null);
    expect(wrapper.prop('loading')).toEqual(false);
  });

  it('initialFormState returns form', () => {
    const wrapper = shallow(<CreateUser />);

    expect(wrapper.state('success')).toBe(null);
    expect(wrapper.state('error')).toBe(null);
    expect(wrapper.state('loading')).toBe(false);

    checkIfFormEqualsInitialState(wrapper);
  });

  it('handleFormChange changes state of form', () => {
    const wrapper = shallow(<CreateUser />);

    checkIfFormEqualsInitialState(wrapper);

    wrapper
      .instance()
      .handleFormChange({ target: { name: 'forename', value: 'test' } });
    wrapper
      .instance()
      .handleFormChange({ target: { name: 'surname', value: '12345' } });
    expect(wrapper.state('form').forename).toEqual('test');
    expect(wrapper.state('form').surname).toEqual('12345');
  });

  it('handleStartDateChange changes state of startDate', () => {
    const wrapper = shallow(<CreateUser />);

    checkIfFormEqualsInitialState(wrapper);

    const newDate = moment();
    wrapper.instance().handleStartDateChange(newDate);
    expect(wrapper.state('form').startDate).toEqual(newDate);
    expect(wrapper.state('form').startDate).toBeInstanceOf(moment);
  });

  it('handleFormSubmit sets state of loading to true', () => {
    const wrapper = shallow(<CreateUser />);

    wrapper.instance().handleFormSubmit();

    expect(wrapper.state('loading')).toBe(true);
  });

  it('when handleFormSubmit submits successfully it sets state of loading to false and success to true', async () => {
    const wrapper = shallow(<CreateUser />);
    const spy = jest.spyOn(userService, 'createUser');

    await wrapper.instance().handleFormSubmit();

    expect(wrapper.state('loading')).toBe(false);
    expect(wrapper.state('success')).toBe(true);

    expect(spy).toHaveBeenCalled();
  });

  it('when handleFormSubmit submits successfully it sets state of the form back to initial state on submission', async () => {
    const wrapper = shallow(<CreateUser />);

    await wrapper.instance().handleFormSubmit();

    checkIfFormEqualsInitialState(wrapper);
  });

  it('when handleFormSubmit does not submits successfully it sets state of loading to false and error to the error returned from createUser', async () => {
    const wrapper = shallow(<CreateUser />);

    //cause test to fail with null value
    wrapper.setState({
      form: { ...wrapper.state('form'), forename: null },
    });

    await wrapper.instance().handleFormSubmit();
    expect(wrapper.state('loading')).toBe(false);
    expect(wrapper.state('error')).toEqual('error');
  });
});

function checkIfFormEqualsInitialState(wrapper) {
  const form = wrapper.instance().initialFormState();

  expect(form).toHaveProperty('forename', '');
  expect(form).toHaveProperty('surname', '');
  expect(form).toHaveProperty('email', '');
  expect(form).toHaveProperty('password', '');
  expect(form).toHaveProperty('country', 1);
  expect(form).toHaveProperty('status', 1);
  expect(form).toHaveProperty('employeeRole', 1);
  expect(form.startDate).toBeInstanceOf(moment);
}
