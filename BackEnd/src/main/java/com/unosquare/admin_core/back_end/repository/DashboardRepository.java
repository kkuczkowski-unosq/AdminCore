package com.unosquare.admin_core.back_end.repository;

import com.unosquare.admin_core.back_end.entity.Client;
import com.unosquare.admin_core.back_end.entity.Event;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;


public interface DashboardRepository extends JpaRepository<Event, Integer> {

}