import React from 'react';
import { PropTypes as PT } from 'prop-types';
import container from './container';
import CreateClientForm from './CreateClientForm';
import { Button } from '../common';
import { CornerButton } from '../common_styled';

export const CreateClient = props => {
  const { clientId, updateClient, createClient, history } = props;

  return (
    <div>
      <h2>{clientId > 0 ? 'Update Client' : 'Create Client'}</h2>
      <CornerButton>
        <Button
          label="View all clients"
          onClick={() => history.push('/admin/clients')}
        />
      </CornerButton>
      <CreateClientForm
        clientId={clientId}
        onRequestUpdate={updateClient}
        onRequestCreate={createClient}
      />
    </div>
  );
};

CreateClient.propTypes = {
  history: PT.object.isRequired,
  clientId: PT.number.isRequired,
  updateClient: PT.func.isRequired,
  createClient: PT.func.isRequired,
};

export default container(CreateClient);
