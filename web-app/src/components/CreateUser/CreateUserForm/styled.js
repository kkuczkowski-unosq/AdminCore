import styled from 'styled-components';

export const ButtonWrap = styled.div`
  display: flex;
  justify-content: flex-end;
  flex-direction: row;
  flex-wrap: nowrap;
  width: 100%;
  height: 40px;
  button {
    flex: 0 1 auto;
    width: 100%;
    max-width: 100%;
    @media (min-width: ${props => props.theme.mediaQueries.lg}) {
      width: 200px;
    }
  }
`;

export const FormContainer = styled.div`
  form {
    @media (min-width: ${props => props.theme.mediaQueries.lg}) {
      margin-right: -10px;
      margin-left: -10px;
    }

    & > div:not(${ButtonWrap}) {
      @media (min-width: ${props => props.theme.mediaQueries.lg}) {
        margin: 0 0 20px 0;
        display: inline-block;
        width: 50%;
        padding: 0 10px;
        box-sizing: border-box;
        vertical-align: bottom;
      }
    }
  }
`;
