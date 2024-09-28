import { createContext, FC, PropsWithChildren, useContext, useTransition } from "react";

interface TransitionContextType extends ReturnType<typeof useTransition> {}

const TransitionContext = createContext<TransitionContextType>({} as TransitionContextType);

export const TransitionContextProvider: FC<PropsWithChildren> = ({ children }) => {
  const transition = useTransition();

  return <TransitionContext.Provider value={transition}>{children}</TransitionContext.Provider>;
};

export const useTransitionContext = () => {
  const cntx = useContext(TransitionContext);

  return cntx;
};
