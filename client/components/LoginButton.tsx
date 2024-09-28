"use client";

import { useAuth } from "@/contexts/authContext";
import { useTransitionContext } from "@/contexts/transitionContext";
import { Button } from "flowbite-react";
import { FC } from "react";

export const LoginButton: FC = () => {
  const { isAuth, auth, token, isAuthenticating } = useAuth();
  const [isPending] = useTransitionContext();

  return (
    <Button
      type="button"
      onClick={auth}
      isProcessing={isAuthenticating || isPending}
      disabled={isAuthenticating || isPending}
      title={token}
    >
      {!isAuth ? "Войти" : "Перевойти"}
    </Button>
  );
};
