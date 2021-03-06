import React from 'react';
import { Text } from 'react-native';
import { PropTypes as PT } from 'prop-types';
import styles from './styles';

const P = (props) => {
  const { children, type, style } = props;

  return (
    <Text {...props} style={[styles[type], style]}>{children}</Text>
  );
};

P.defaultProps = {
  type: 'base',
};

P.propTypes = {
  children: PT.node.isRequired,
  type: PT.string,
  style: PT.oneOfType([
    PT.number,
    PT.object,
    PT.array,
  ]),
};

export default P;
