"use client";

import { useAuth } from "@/contexts/authContext";
import { useTransitionContext } from "@/contexts/transitionContext";
import { Button, Dropdown } from "flowbite-react";
import { FC } from "react";

export const LoginButton: FC = () => {
  const { isAuth, auth, logout, token, isAuthenticating } = useAuth();
  const [isPending] = useTransitionContext();

  const disabled = isAuthenticating || isPending;

  if (!isAuth) {
    return (
      <Button type="button" onClick={auth} isProcessing={isAuthenticating} disabled={disabled} title={token}>
        Войти
      </Button>
    );
  }

  return (
    <Dropdown arrowIcon={false} inline label="Пользователь">
      <Dropdown.Item onClick={auth} disabled={disabled}>
        Новый пользователь
      </Dropdown.Item>
      <Dropdown.Divider />
      <Dropdown.Item onClick={logout} disabled={disabled}>
        Выйти
      </Dropdown.Item>
    </Dropdown>
  );
};
