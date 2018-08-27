import React from 'react';
import { PropTypes as PT } from 'prop-types';
import { Banner } from './styled';
import { statusText } from '../../../utilities/holidayStatus';
import eventTypes, { getEventTypeValue } from '../../../utilities/eventTypes';
import FontAwesomeIcon from '@fortawesome/react-fontawesome';
import { faTrash } from '@fortawesome/fontawesome-free-solid';

const ModalStatusBanner = props => {
  const { userName, eventStatus, eventType, cancelEvent } = props;
  const { eventStatusId, description } = eventStatus;
  const { eventTypeId } = eventType;

  let bannerId;
  let bannerDescription;
  if (eventTypeId === eventTypes.ANNUAL_LEAVE) {
    bannerId = eventStatusId;
    bannerDescription = description;
  } else {
    const selectedEventType = getEventTypeValue(eventTypeId, eventStatusId);
    bannerId = selectedEventType;
    bannerDescription = statusText[selectedEventType];
  }

  return (
    <Banner status={bannerId}>
      <div>
        <h4>{userName}</h4>
        <p>{bannerDescription}</p>
      </div>
      <div>
        <div className="cancelEvent" onClick={cancelEvent}>
          <FontAwesomeIcon icon={faTrash} />
          <span>Cancel Event</span>
        </div>
      </div>
    </Banner>
  );
};

ModalStatusBanner.propTypes = {
  userName: PT.string.isRequired,
  eventStatus: PT.object.isRequired,
  eventType: PT.object.isRequired,
  cancelEvent: PT.func.isRequired,
};

export default ModalStatusBanner;