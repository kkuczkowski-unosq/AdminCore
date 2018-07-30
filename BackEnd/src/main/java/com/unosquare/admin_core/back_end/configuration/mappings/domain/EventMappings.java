package com.unosquare.admin_core.back_end.configuration.mappings.domain;

import com.unosquare.admin_core.back_end.configuration.mappings.BaseMappings;
import com.unosquare.admin_core.back_end.dto.EventDTO;
import com.unosquare.admin_core.back_end.entity.*;
import lombok.NoArgsConstructor;
import org.modelmapper.PropertyMap;

@NoArgsConstructor
public class EventMappings implements BaseMappings<EventDTO, Event> {
    @Override
    public PropertyMap<EventDTO, Event> MapFromSourceToTarget() {
        return new PropertyMap <EventDTO, Event>() {
            protected void configure() {
                map().setHalfDay(source.isHalfDay());
                map().setEventStatus(new EventStatus(source.getEventStatusId()));
                map().setEventType(new EventType(source.getEventTypeId()));

            }
        };
    }

    @Override
    public PropertyMap<Event, EventDTO> MapFromTargetToSource() {
        return new PropertyMap <Event, EventDTO>() {
            protected void configure() {
                map().setEventStatusId(source.getEventStatus().getEventStatusId());
                map().setEventTypeId(source.getEventType().getEventTypeId());
                map().setHalfDay(source.isHalfDay());
            }
        };
    }
}