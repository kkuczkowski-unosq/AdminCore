import React from 'react';
import { PropTypes as PT } from 'prop-types';
import ReactTable from 'react-table';
import TableValues from './TableValues';

const buildColumns = columns => {
  const formattedColumns = columns.reduce((acc, column) => {
    return acc.concat(TableValues[column]);
  }, []);

  return formattedColumns;
};

const renderTable = (contracts, columns, onRowClick) => {
  const formattedColumns = buildColumns(columns);
  return (
    <ReactTable
      data={contracts}
      columns={formattedColumns}
      defaultPageSize={10}
      className="-striped -highlight"
      getTrProps={(state, rowInfo) => {
        return {
          onClick: () => onRowClick(rowInfo.original),
          style: {
            cursor: rowInfo ? 'pointer' : 'null',
          },
        };
      }}
    />
  );
};

export const ContractList = props => {
  const { contracts, columns, onRowClick } = props;
  return !contracts || contracts.length === 0 ? (
    <p>There are no contracts to show</p>
  ) : (
    renderTable(contracts, columns, onRowClick)
  );
};

ContractList.propTypes = {
  contracts: PT.array,
  columns: PT.array.isRequired,
  onRowClick: PT.func,
};

ContractList.defaultProps = {
  onRowClick: () => {},
};

export default ContractList;
