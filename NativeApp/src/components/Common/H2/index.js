import React from 'react';
import { PropTypes as PT } from 'prop-types';
import { Text } from 'react-native';
import styles from './styles';

const H2 = (props) => {
  const { children, styleProp, type } = props;

  return (
    <Text style={[styles[type], styleProp]} {...props}>{children}</Text>
  );
};

H2.propTypes = {
  children: PT.node.isRequired,
  type: PT.string.isRequired,
  styleProp: PT.oneOfType([
    PT.number,
    PT.object,
    PT.array,
  ]),
};

export default H2;