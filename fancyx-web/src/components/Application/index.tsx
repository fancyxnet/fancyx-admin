import { App } from 'antd';
import React, { createContext, useContext, useEffect } from 'react';
import UserStore from '@/store/userStore';

export interface ApplicationContextType {
}

const ApplicationContext = createContext<ApplicationContextType>({ });
const Application = ({ children }: { children: React.ReactNode }) => {

  useEffect(() => {
    if (UserStore.isAuthenticated()) {
      UserStore.startTokenChecker();
    }

    return () => {
      UserStore.stopTokenChecker();
    };
  }, []);

  return (
    <ApplicationContext.Provider value={{  }}>
      <App>{children}</App>
    </ApplicationContext.Provider>
  );
};

export default Application;

// eslint-disable-next-line react-refresh/only-export-components
export const useApplication = () => useContext(ApplicationContext);
